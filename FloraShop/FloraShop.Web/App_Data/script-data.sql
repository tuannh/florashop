SET IDENTITY_INSERT [dbo].[Brands] ON 

INSERT [dbo].[Brands] ([Id], [Alias], [Name], [Description], [Active], [DisplayOrder], [Photo]) VALUES (1, N'levis', N'Levis''s', NULL, 1, 1000, NULL)
INSERT [dbo].[Brands] ([Id], [Alias], [Name], [Description], [Active], [DisplayOrder], [Photo]) VALUES (2, N'gucci', N'Gucci', NULL, 1, 1000, NULL)
INSERT [dbo].[Brands] ([Id], [Alias], [Name], [Description], [Active], [DisplayOrder], [Photo]) VALUES (3, N'nike', N'Nike', NULL, 1, 1000, NULL)
SET IDENTITY_INSERT [dbo].[Brands] OFF
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([Id], [Alias], [Name], [Description], [Active], [ParentId], [DisplayOrder]) VALUES (1, N'ao', N'Áo', NULL, 1, NULL, 1000)
INSERT [dbo].[Categories] ([Id], [Alias], [Name], [Description], [Active], [ParentId], [DisplayOrder]) VALUES (2, N'quan', N'Quần', NULL, 1, NULL, 1000)
INSERT [dbo].[Categories] ([Id], [Alias], [Name], [Description], [Active], [ParentId], [DisplayOrder]) VALUES (3, N'dam', N'Đầm', NULL, 1, 1, 1000)
SET IDENTITY_INSERT [dbo].[Categories] OFF
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([Id], [Name], [Alias], [Code], [ShortDescription], [Description], [Active], [Price], [SalePrice], [MadeIn], [Sizes], [DislayOrder], [CategoryId], [BrandId], [Type]) VALUES (1, N'Sản phẩm 1', N'san-pham-1', N'sp1', N'Sản phẩm 1', N'asadfasd fsfsdfsa sf', 1, 0, 0, N'Made in VN', NULL, 1000, 1, 1, 1)
INSERT [dbo].[Products] ([Id], [Name], [Alias], [Code], [ShortDescription], [Description], [Active], [Price], [SalePrice], [MadeIn], [Sizes], [DislayOrder], [CategoryId], [BrandId], [Type]) VALUES (2, N'Sản phẩm 2', N'san-pham-2', N'sp2', N'Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2', N'Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2', 1, 0, 0, NULL, NULL, 1000, 2, 1, 1)
SET IDENTITY_INSERT [dbo].[Products] OFF
SET IDENTITY_INSERT [dbo].[Banners] ON 

INSERT [dbo].[Banners] ([Id], [Name], [Description], [FileName], [Active], [Target], [Url], [DisplayOrder]) VALUES (1, N'Banner 1', N'<div class="infor-sale">
<h4>Khuyến m&atilde;i</h4>
<br/>

<h3><a href="#">Giảm 50% c&aacute;c loại v&aacute;y</a></h3>
</div>

<div class="infor-news">
<h4>Sản phẩm mới</h4>
<br/>

<h3><a href="#">V&aacute;y Gucci Summer 2014</a></h3>
<br/>

<h3><a href="#">Jean nữ Marc Jacob</a></h3>
</div>
', N'1-banner1.jpg', 1, N'_blank', NULL, 1000)
INSERT [dbo].[Banners] ([Id], [Name], [Description], [FileName], [Active], [Target], [Url], [DisplayOrder]) VALUES (2, N'Banner 2', N'<div class="infor-sale">
<h4>Khuyến m&atilde;i</h4>
<br/>

<h3><a href="#">Giảm 50% c&aacute;c loại v&aacute;y</a></h3>
</div>

<div class="infor-news">
<h4>Sản phẩm mới</h4>
<br/>

<h3><a href="#">V&aacute;y Gucci Summer 2014</a></h3>
<br/>

<h3><a href="#">Jean nữ Marc Jacob</a></h3>
</div>', N'2-banner2.jpg', 1, N'_blank', NULL, 1000)
SET IDENTITY_INSERT [dbo].[Banners] OFF
SET IDENTITY_INSERT [dbo].[Contents] ON 

INSERT [dbo].[Contents] ([Id], [Name], [Alias], [Value], [PageAlias]) VALUES (1, N'Hướng dẫn<br/>&nbsp;&nbsp;mua hàng', N'', N'fasdfasdf', N'huong-dan-mua-hang')
INSERT [dbo].[Contents] ([Id], [Name], [Alias], [Value], [PageAlias]) VALUES (3, N'Liên hệ<br/>&nbsp;&nbsp;chúng tôi', N'', N'adfasd', N'lien-he')
SET IDENTITY_INSERT [dbo].[Contents] OFF
SET IDENTITY_INSERT [dbo].[Pages] ON 

INSERT [dbo].[Pages] ([Id], [UniqueKey], [Title], [Name], [Alias], [IsDefault], [Active], [MetaKeyword], [MetaDescription], [Layout]) VALUES (1, N'Home', N'Trang chủ', N'Trang chủ', N'index', 1, 1, NULL, NULL, N'~/Views/Layouts/Index.cshtml')
INSERT [dbo].[Pages] ([Id], [UniqueKey], [Title], [Name], [Alias], [IsDefault], [Active], [MetaKeyword], [MetaDescription], [Layout]) VALUES (2, N'Product', N'Sản phẩm', N'Sản phẩm', N'san-pham', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT [dbo].[Pages] ([Id], [UniqueKey], [Title], [Name], [Alias], [IsDefault], [Active], [MetaKeyword], [MetaDescription], [Layout]) VALUES (3, N'BuyGuide', N'Hướng dẫn mua hàng', N'Hướng dẫn mua hàng', N'huong-dan-mua-hang', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT [dbo].[Pages] ([Id], [UniqueKey], [Title], [Name], [Alias], [IsDefault], [Active], [MetaKeyword], [MetaDescription], [Layout]) VALUES (4, N'Promotion', N'Khuyến mãi', N'Khuyến mãi', N'khuyen-mai', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT [dbo].[Pages] ([Id], [UniqueKey], [Title], [Name], [Alias], [IsDefault], [Active], [MetaKeyword], [MetaDescription], [Layout]) VALUES (5, N'Contact', N'Liên hệ', N'Liên hệ', N'lien-he', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
SET IDENTITY_INSERT [dbo].[Pages] OFF
