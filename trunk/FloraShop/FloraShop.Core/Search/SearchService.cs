using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IO = System.IO;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Analysis;
using FloraShop.Core.Domain;
using Claw.Modules.System.SearchHelper;
using FloraShop.Core.DAL;
using FloraShop.Core;


namespace FloraShop.Core.Search
{
    public static class SearchService
    {
        const int HitsLimit = 1000;

        public const string SearchCacheKeyPrefix = "LuceneSearch";

        public static void ClearSearchResultCache()
        {
            FloraShop.Core.SiteCache.RemoveByPattern("SearchCacheKey");
        }

        const string DefaultIndexFolder = "~/Userfiles/Index/Modules/Products/";

        #region attribute

        private static bool _customKeyword = false;

        private static string _luceneDir = null;

        private static FSDirectory _directoryTemp;

        private static FSDirectory _directory
        {
            get
            {
                _makeIndexFolder();

                if (_directoryTemp == null)
                    _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));

                if (IndexWriter.IsLocked(_directoryTemp))
                    IndexWriter.Unlock(_directoryTemp);

                var lockFilePath = Path.Combine(_luceneDir, "write.lock");

                if (File.Exists(lockFilePath))
                    File.Delete(lockFilePath);

                return _directoryTemp;
            }
        }

        #endregion

        #region support methods

        private static bool _isLuceneIndexing()
        {
            var lockFilePath = Path.Combine(_luceneDir, "write.lock");
            return File.Exists(lockFilePath);
        }

        private static Analyzer _getAnalyzer()
        {
            // return new Lucene.Net.Analysis.Shingle.ShingleAnalyzerWrapper(new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30), SearchEngine.MaxSuggestWord);
            return new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
        }

        private static void _addToLuceneIndex(Product product, IndexWriter writer)
        {
            var term = new Term(IndexKeys.Id, product.Id.ToString());
            writer.DeleteDocuments(term);

            // add new index entry
            var doc = new Document();

            // add lucene fields mapped to db fields
            doc.Add(new Field(IndexKeys.Id, product.Id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field(IndexKeys.Active, product.Active.ToString(), Field.Store.NO, Field.Index.NOT_ANALYZED));

            doc.Add(new Field(IndexKeys.Name, product.Name, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field(IndexKeys.Alias, product.Alias, Field.Store.YES, Field.Index.NO));

            doc.Add(new Field(IndexKeys.Code, product.Code, Field.Store.NO, Field.Index.NOT_ANALYZED));

            if (!string.IsNullOrEmpty(product.ShortDescription))
                doc.Add(new Field(IndexKeys.ShortDescription, product.ShortDescription, Field.Store.YES, Field.Index.NOT_ANALYZED));

            if (!string.IsNullOrEmpty(product.Description))
                doc.Add(new Field(IndexKeys.Description, product.Description, Field.Store.NO, Field.Index.NOT_ANALYZED));

            if (product.Price > 0)
                doc.Add(new Field(IndexKeys.Price, product.Price.ToString("#########"), Field.Store.YES, Field.Index.NOT_ANALYZED));

            if (product.SalePrice > 0)
                doc.Add(new Field(IndexKeys.SalePrice, product.SalePrice.ToString("#########"), Field.Store.YES, Field.Index.NOT_ANALYZED));

            if (!string.IsNullOrEmpty(product.MadeIn))
                doc.Add(new Field(IndexKeys.MadeIn, product.MadeIn.ToString(), Field.Store.YES, Field.Index.ANALYZED));

            var cate = product.Category;
            while (cate != null)
            {
                doc.Add(new Field(IndexKeys.CategoryId, cate.Alias.ToLower(), Field.Store.NO, Field.Index.NOT_ANALYZED));
                cate = cate.Parent;
            }

            if (product.Brand != null)
                doc.Add(new Field(IndexKeys.BrandId, product.Brand.Alias.ToLower(), Field.Store.NO, Field.Index.NOT_ANALYZED));

            doc.Add(new Field(IndexKeys.Type, product.Type.ToString(), Field.Store.NO, Field.Index.NOT_ANALYZED));

            var photo = product.GetPhoto();
            if (photo != null)
            {
                doc.Add(new Field(IndexKeys.Photo, photo.FileName, Field.Store.YES, Field.Index.NO));

            }

            // add entry to index
            writer.AddDocument(doc);
        }

        private static SearchItem _mapLuceneDocumentToData(Query query, Document doc)
        {
            var strPrice = doc.Get(IndexKeys.Price);
            var strSalePrice = doc.Get(IndexKeys.SalePrice);

            float price = 0;
            float salePrice = 0;
            float.TryParse(strPrice, out price);
            float.TryParse(strSalePrice, out salePrice);

            var search = new SearchItem()
            {
                Id = doc.Get(IndexKeys.Id),
                Name = doc.Get(IndexKeys.Name),
                Alias = doc.Get(IndexKeys.Alias),
                Description = doc.Get(IndexKeys.ShortDescription),
                Price = price,
                SalePrice = salePrice,
                Photo =  doc.Get(IndexKeys.Photo)
            };

            return search;
        }

        private static IEnumerable<SearchItem> _mapLuceneToDataList(Query query, IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
        {
            return hits.Select(hit => _mapLuceneDocumentToData(query, searcher.Doc(hit.Doc))).ToList();
        }

        private static Query ParseQuery(string searchQuery, QueryParser parser)
        {
            Query query;
            try
            {
                query = parser.Parse(searchQuery.Trim());
            }
            catch (ParseException)
            {
                query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
            }

            return query;
        }

        private static IEnumerable<SearchItem> _search(string keyword, string cateAlias, string brandAlias, int type)
        {
            // set up lucene searcher
            using (var searcher = new IndexSearcher(_directory, false))
            {
                using (var analyzer = _getAnalyzer())
                {
                    var filter = new BooleanFilter();

                    #region search filter

                    var activeFilter = new TermsFilter();
                    activeFilter.AddTerm(new Term(IndexKeys.Active, true.ToString()));
                    filter.Add(new FilterClause(activeFilter, Occur.MUST));

                    if (!string.IsNullOrEmpty(cateAlias))
                    {
                        var catFilter = new TermsFilter();
                        catFilter.AddTerm(new Term(IndexKeys.CategoryId, cateAlias.ToLower()));
                        filter.Add(new FilterClause(catFilter, Occur.MUST));
                    }

                    if (!string.IsNullOrEmpty(brandAlias))
                    {
                        var brandFilter = new TermsFilter();
                        brandFilter.AddTerm(new Term(IndexKeys.BrandId, brandAlias.ToLower()));
                        filter.Add(new FilterClause(brandFilter, Occur.MUST));
                    }

                    if (type > 0)
                    {
                        var typeFilter = new TermsFilter();
                        typeFilter.AddTerm(new Term(IndexKeys.Type, type.ToString()));
                        filter.Add(new FilterClause(typeFilter, Occur.MUST));
                    }

                    #endregion

                    var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30,
                                                                new[] { IndexKeys.Name, IndexKeys.Code, IndexKeys.ShortDescription,
                                                                            IndexKeys.Description, IndexKeys.MadeIn, IndexKeys.Price,
                                                                            IndexKeys.SalePrice, IndexKeys.CategoryId, IndexKeys.BrandId},
                                                                analyzer);

                    Query query = null;
                    if (string.IsNullOrEmpty(keyword))
                    {
                        query = new MatchAllDocsQuery(IndexKeys.Name);
                    }
                    else
                    {
                        query = ParseQuery(keyword, parser);
                    }

                    var hits = searcher.Search(query, filter, SearchService.HitsLimit, Sort.RELEVANCE).ScoreDocs;
                    var results = _mapLuceneToDataList(query, hits, searcher);

                    analyzer.Close();

                    return results;
                }
            }
        }

        static void _makeIndexFolder()
        {
            if (!IO.Directory.Exists(_luceneDir))
                IO.Directory.CreateDirectory(_luceneDir);
        }

        #endregion

        static SearchService()
        {
            _luceneDir = Globals.MapPath(DefaultIndexFolder);
        }

        /// <summary>
        /// Initializes the index records for the first time. If reIndex = true it's reindex.
        /// </summary>
        /// <param name="reIndex">if set to <c>true</c> [re index].</param>
        public static void InitIndexRecords(bool reIndex = false)
        {
            _makeIndexFolder();

            if (IO.Directory.GetFiles(_luceneDir).Length == 0 || reIndex)
            {
                var db = new FloraShopContext();

                var lst = db.Products.Include(a => a.Category).Include(b => b.Brand).Where(p => p.Active).ToList();

                if (lst != null)
                    AddUpdateLuceneIndex(lst);
            }
        }

        public static void AddUpdateLuceneIndex(IEnumerable<Product> list)
        {
            using (var analyzer = _getAnalyzer())
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entry if any)
                    foreach (var obj in list)
                        _addToLuceneIndex(obj, writer);

                    // close handles
                    analyzer.Close();
                }
            }

            Optimize();
        }

        public static void AddUpdateLuceneIndex(Product product, bool optimize = true)
        {
            // init lucene
            using (var analyzer = _getAnalyzer())
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entry if any)
                    _addToLuceneIndex(product, writer);

                    if (optimize)
                        writer.Optimize();
                }
            }

            SearchService.ClearSearchResultCache();
        }

        public static void ClearRecordIndex(Product obj)
        {
            if (obj != null)
                ClearRecordIndex(obj.Id);
        }

        private static void ClearRecordIndex(int productId)
        {
            using (var analyzer = _getAnalyzer())
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    var term = new Term(IndexKeys.Id, productId.ToString());

                    writer.DeleteDocuments(term);
                    writer.Optimize();
                    analyzer.Close();
                }
            }

            SearchService.ClearSearchResultCache();
        }

        public static void ClearAllIndex()
        {
            using (var analyzer = _getAnalyzer())
            {
                using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entries
                    writer.DeleteAll();
                    writer.Optimize();

                    // close handles
                    analyzer.Close();
                }
            }

            SearchService.ClearSearchResultCache();
        }

        public static void Optimize()
        {
            using (var analyzer = _getAnalyzer())
            {
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    writer.Optimize();
                    analyzer.Close();
                }
            }
        }

        public static IEnumerable<SearchItem> Search(string keyword, string cateAlias, string brandAlias, int type)
        {
            if (_isLuceneIndexing())
                return new List<SearchItem>();

            InitIndexRecords();

            if (!string.IsNullOrEmpty(keyword))
                keyword = keyword.Trim();

            var cackeKey = string.Format("{0}-cate:{1}-brand:{2}-type:{3}-kw:{4}", SearchCacheKeyPrefix, cateAlias, brandAlias, type, Globals.GenerateAlias(keyword));

            var lst = SiteCache.Get(cackeKey) as IEnumerable<SearchItem>;
            if (lst == null || lst.Count() == 0)
            {
                lst = _search(keyword, cateAlias, brandAlias, type);

            }

            return lst;
        }
    }
}
