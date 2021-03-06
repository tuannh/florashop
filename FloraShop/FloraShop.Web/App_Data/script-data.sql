/*
	SQL Server Dumper

	User Interface: SQL Server Dumper  3.0.8
	Script Engine:  SQLDumper.Engine  1.0.8

	Copyright © 2009 Ruizata Project. All Rights Reserved.

	Creation Date:2014-12-22 11:59:57
	Database:`FloraShop` 
*/


-- `dbo.Banners`
SET IDENTITY_INSERT dbo.Banners ON

INSERT dbo.Banners (Id, Name, Description, FileName, Active, Target, Url, DisplayOrder, Category) VALUES (1, N'Banner 1', N'<div class="infor-sale">
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
', N'1-banner1.jpg', 1, N'_blank', NULL, 1000, 0)
INSERT dbo.Banners (Id, Name, Description, FileName, Active, Target, Url, DisplayOrder, Category) VALUES (2, N'Banner 2', N'<div class="infor-sale">
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
</div>', N'2-banner2.jpg', 1, N'_blank', NULL, 1000, 0)
INSERT dbo.Banners (Id, Name, Description, FileName, Active, Target, Url, DisplayOrder, Category) VALUES (3, N'Banner 1', NULL, N'3-dt-huu-tuyen.jpg', 1, N'_blank', NULL, 1000, 1)
INSERT dbo.Banners (Id, Name, Description, FileName, Active, Target, Url, DisplayOrder, Category) VALUES (4, N'Banner home 1', NULL, N'4-gia-dung.jpg', 1, N'_blank', NULL, 1000, 0)
INSERT dbo.Banners (Id, Name, Description, FileName, Active, Target, Url, DisplayOrder, Category) VALUES (5, N'Sản phẩm 3', NULL, N'5-dien-lanh.jpg', 1, N'_blank', N'http://www.google.com.vn', 1000, 1)
SET IDENTITY_INSERT dbo.Banners OFF


-- `dbo.Brands`
SET IDENTITY_INSERT dbo.Brands ON

INSERT dbo.Brands (Id, [Alias], Name, Description, Active, DisplayOrder, Photo) VALUES (1, N'levis', N'Levis''s', NULL, 1, 1000, NULL)
INSERT dbo.Brands (Id, [Alias], Name, Description, Active, DisplayOrder, Photo) VALUES (2, N'gucci', N'Gucci', NULL, 1, 1000, NULL)
INSERT dbo.Brands (Id, [Alias], Name, Description, Active, DisplayOrder, Photo) VALUES (3, N'nike', N'Nike', NULL, 1, 1000, NULL)
SET IDENTITY_INSERT dbo.Brands OFF


-- `dbo.Categories`
SET IDENTITY_INSERT dbo.Categories ON

ALTER TABLE dbo.Categories NOCHECK CONSTRAINT ALL

INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (1, N'ao', N'Áo', NULL, 1, NULL, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (2, N'quan', N'Quần', NULL, 1, NULL, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (3, N'dam', N'Đầm', NULL, 1, 1, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (4, N'ao-khoac', N'Áo khoác', NULL, 1, 1, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (5, N'ao-thun', N'Áo thun', NULL, 1, 1, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (6, N'quan-jean', N'Quần jean', NULL, 1, 2, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (7, N'quan-short', N'Quần short', NULL, 1, 2, 1000)
INSERT dbo.Categories (Id, [Alias], Name, Description, Active, ParentId, DisplayOrder) VALUES (8, N'quan-leg', N'Quần leg', NULL, 1, 2, 1000)
ALTER TABLE dbo.Categories CHECK CONSTRAINT ALL

SET IDENTITY_INSERT dbo.Categories OFF


-- `dbo.Contents`
SET IDENTITY_INSERT dbo.Contents ON

INSERT dbo.Contents (Id, Name, [Alias], [Value], PageAlias) VALUES (1, N'Hướng dẫn<br/>&nbsp;&nbsp;mua hàng', N'', N'fasdfasdf', N'huong-dan-mua-hang')
INSERT dbo.Contents (Id, Name, [Alias], [Value], PageAlias) VALUES (2, N'Liên hệ<br/>&nbsp;&nbsp;chúng tôi', N'', N'adfasd', N'lien-he')
INSERT dbo.Contents (Id, Name, [Alias], [Value], PageAlias) VALUES (3, N'Hỗ trợ trực tuyến', N'ho-tro-truc-tuyen', N'<p><img alt="" src="/images/flora/skype.png" /> florashopvn</p>

<p><img alt="" src="/images/flora/yahoo.png" /> florashopvn</p>
', NULL)
INSERT dbo.Contents (Id, Name, [Alias], [Value], PageAlias) VALUES (4, N'Contact map', N'contact-map', N'aba', NULL)
INSERT dbo.Contents (Id, Name, [Alias], [Value], PageAlias) VALUES (5, N'Footer contact', N'footer-contact', N'fadf', NULL)
INSERT dbo.Contents (Id, Name, [Alias], [Value], PageAlias) VALUES (6, N'Bottom promotion info', N'bottom-promotion', NULL, NULL)
SET IDENTITY_INSERT dbo.Contents OFF


-- `dbo.Pages`
SET IDENTITY_INSERT dbo.Pages ON

INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (1, N'Home', N'Trang chủ', N'Trang chủ', N'', 1, 1, NULL, NULL, N'~/Views/Layouts/Index.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (2, N'Product', N'Sản phẩm', N'Sản phẩm', N'san-pham', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (3, N'BuyGuide', N'Hướng dẫn mua hàng', N'Hướng dẫn mua hàng', N'huong-dan-mua-hang', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (4, N'Promotion', N'Khuyến mãi', N'Khuyến mãi', N'khuyen-mai', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (5, N'Contact', N'Liên hệ', N'Liên hệ', N'lien-he', 0, 1, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (6, N'ProductDetail', N'Chi tiết sản phẩm', N'Chi tiết sản phẩm', N'chi-tiet-san-pham', 0, 0, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (7, N'Shopcart', N'Giỏ hàng', N'Giỏ hàng', N'gio-hang', 0, 0, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (8, N'Order', N'Đặt hàng', N'Đặt hàng', N'dat-hang', 0, 0, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (9, N'Login', N'Đăng nhập', N'Đăng nhập', N'dang-nhap', 0, 0, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
INSERT dbo.Pages (Id, UniqueKey, Title, Name, [Alias], IsDefault, Active, MetaKeyword, MetaDescription, Layout) VALUES (10, N'Register', N'Đăng ký', N'Đăng ký', N'dang-ky', 0, 0, NULL, NULL, N'~/Views/Layouts/Content.cshtml')
SET IDENTITY_INSERT dbo.Pages OFF


-- `dbo.Provinces`
SET IDENTITY_INSERT dbo.Provinces ON

INSERT dbo.Provinces (Id, Name, [Order]) VALUES (1, N'An Giang', 1)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (2, N'Bà Rịa Vũng Tàu', 2)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (3, N'Bắc Giang', 3)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (4, N'Bắc Kạn', 4)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (5, N'Bạc Liêu', 5)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (6, N'Bắc Ninh', 6)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (7, N'Bến Tre', 7)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (8, N'Bình Dương', 8)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (9, N'Bình Phước', 9)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (10, N'Bình Thuận', 10)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (11, N'Bình Định', 11)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (12, N'Cà Mau', 12)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (13, N'Cần Thơ', 13)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (14, N'Cao Bằng', 14)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (15, N'Gia Lai', 15)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (16, N'Hà Giang', 16)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (17, N'Hà Nam', 17)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (18, N'Hà Nội', 18)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (19, N'Hà Tây', 19)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (20, N'Hà Tĩnh', 20)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (21, N'Hải Dương', 21)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (22, N'Hải Phòng', 22)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (23, N'Hậu Giang', 23)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (24, N'Hồ Chí Minh', 24)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (25, N'Hoà Bình', 25)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (26, N'Huế', 26)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (27, N'Hưng Yên', 27)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (28, N'Khánh Hoà', 28)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (29, N'Kiên Giang', 29)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (30, N'Kon Tum', 30)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (31, N'Lai Châu', 31)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (32, N'Lâm Ðồng', 32)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (33, N'Lạng Sơn', 33)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (34, N'Lào Cai', 34)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (35, N'Long An', 35)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (36, N'Nam Ðịnh', 36)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (37, N'Nghệ An', 37)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (38, N'Ninh Bình', 38)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (39, N'Ninh Thuận', 39)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (40, N'Phú Thọ', 40)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (41, N'Phú Yên', 41)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (42, N'Quảng Bình', 42)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (43, N'Quảng Nam', 43)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (44, N'Quảng Ngãi', 44)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (45, N'Quảng Ninh', 45)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (46, N'Quảng Trị', 46)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (47, N'Sóc Trăng', 47)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (48, N'Sơn La', 48)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (49, N'Tây Ninh', 49)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (50, N'Thái Bình', 50)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (51, N'Thái Nguyên', 51)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (52, N'Thanh Hóa', 52)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (53, N'Tiền Giang', 53)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (54, N'Toàn Quốc', 54)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (55, N'Trà Vinh', 55)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (56, N'Tuyên Quang', 56)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (57, N'Vĩnh Long', 57)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (58, N'Vĩnh Phúc', 58)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (59, N'Yên Bái', 59)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (60, N'Ðà Nẵng', 60)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (61, N'Ðắc Lắc', 61)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (62, N'Ðắk Nông', 62)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (63, N'Ðồng Nai', 63)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (64, N'Ðồng Tháp', 64)
INSERT dbo.Provinces (Id, Name, [Order]) VALUES (65, N'Điện Biên', 65)
SET IDENTITY_INSERT dbo.Provinces OFF


-- `dbo.UserGuides`

-- `dbo.Districts`
SET IDENTITY_INSERT dbo.Districts ON

INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (1, N'Bình Chánh', 1, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (2, N'Bình Tân', 2, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (3, N'Bình Thạnh', 3, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (4, N'Cần Giờ', 4, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (5, N'Củ Chi', 5, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (6, N'Gò vấp', 6, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (7, N'Hóc Môn', 7, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (8, N'KDC Trung Sơn (Bình Chánh)', 8, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (9, N'Nhà Bè', 9, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (10, N'Phú Nhuận', 10, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (11, N'Quận 1', 11, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (12, N'Quận 10', 12, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (13, N'Quận 11', 13, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (14, N'Quận 12', 14, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (15, N'Quận 2', 15, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (16, N'Quận 3', 16, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (17, N'Quận 4', 17, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (18, N'Quận 5', 18, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (19, N'Quận 6', 19, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (20, N'Quận 7', 20, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (21, N'Quận 8', 21, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (22, N'Quận 9', 22, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (23, N'Tân Bình', 23, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (24, N'Tân Phú', 24, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (25, N'Thủ Đức', 25, 24)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (26, N'Ba Ðình', 1, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (27, N'Cầu Giấy', 2, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (28, N'Gia Lâm', 3, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (29, N'Hà Ðông', 4, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (30, N'Hai Bà Trung', 5, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (31, N'Hoài Ðức', 6, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (32, N'Hoàn Kiếm', 7, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (33, N'Hoàng Mai', 8, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (34, N'Long Biên', 9, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (35, N'Mê Linh', 10, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (36, N'Sóc Sơn', 11, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (37, N'Sơn Tây', 12, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (38, N'Tây Hồ', 13, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (39, N'Thanh Trì', 14, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (40, N'Thanh Xuân', 15, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (41, N'Từ Liêm', 16, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (42, N'Ðông Anh', 17, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (43, N'Ðống Ða', 18, 18)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (44, N'Cẩm Lệ', 1, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (45, N'Hải Châu', 2, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (46, N'Hòa Vang', 3, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (47, N'Hoàng Sa', 4, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (48, N'Liên Chiểu', 5, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (49, N'Ngũ Hành Son', 6, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (50, N'Sơn Trà', 7, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (51, N'Thanh Khê', 8, 60)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (52, N'Bình Thủy', 1, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (53, N'Cái Răng', 2, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (54, N'Cờ Ðỏ', 3, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (55, N'Ninh Kiều', 4, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (56, N'Ô Môn', 5, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (57, N'Phong Ðiền', 6, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (58, N'Thới Lai', 7, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (59, N'Thốt Nốt', 8, 13)
INSERT dbo.Districts (Id, Name, [Order], ProvinceId) VALUES (60, N'Vĩnh Thạnh', 9, 13)
SET IDENTITY_INSERT dbo.Districts OFF


-- `dbo.Products`
SET IDENTITY_INSERT dbo.Products ON

INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (1, N'Sản phẩm 1', N'san-pham-1', N'sp1', N'Sản phẩm 1', N'<p>asadfasd fsfsdfsa sf</p>
', 1, 300000, 0, N'Made in VN', NULL, NULL, 1000, 1, 1, 1, '20141210 23:17:26:487')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (2, N'Sản phẩm 2', N'san-pham-2', N'sp2', N'Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2', N'<p>Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2Sản phẩm 2</p>
', 1, 200000, 0, NULL, NULL, NULL, 1000, 2, 1, 1, '20141210 23:17:10:103')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (3, N'Sản phẩm 3', N'san-pham-3', N'sp3', N'Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3', N'<p>Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3Sản phẩm 3 Sản phẩm 3</p>
', 1, 100000, 0, N'Made in VN', NULL, NULL, 1000, 1, 1, 1, '20141208 22:18:28:157')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (4, N'Sản phẩm 4', N'san-pham-4', N'sp4', N'Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 ', N'<p>Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4 Sản phẩm 4</p>
', 1, 700000, 500000, NULL, NULL, NULL, 1000, 6, 2, 1, '20141210 23:16:54:327')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (5, N'Sản phẩm 5', N'san-pham-5', N'sp5', N'Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 ', N'<p>Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5 Sản phẩm 5</p>
', 1, 400000, 0, NULL, N'37; 38; 39; 40', NULL, 1000, 1, 1, 1, '20141210 23:16:37:827')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (6, N'Sản phẩm 6', N'san-pham-6', N'sp6', N'Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5', N'<p>Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5Sản phẩm 5 Sản phẩm 5</p>
', 1, 500000, 350000, N'Made in VN', N'37; 38; 39; 40', N'Đỏ; Xanh; Nâu; Xám', 1000, 4, 2, 1, '20141211 22:47:35:197')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (7, N'Sản phẩm 7', N'san-pham-7', N'sp7', N'Sản phẩm 7 Sản phẩm 7 Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7', N'<p>Sản phẩm 7 Sản phẩm 7 Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7 Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7 Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7Sản phẩm 7 Sản phẩm 7</p>
', 1, 500000, 0, N'Made in Japan', N'37; 38; 39; 40', N'Đỏ; Xanh; Nâu; Xám;Tím; Cam', 1000, 1, 2, 1, '20141211 22:49:03:493')
INSERT dbo.Products (Id, Name, [Alias], Code, ShortDescription, Description, Active, Price, SalePrice, MadeIn, Sizes, Colors, DislayOrder, CategoryId, BrandId, Type, CreatedDate) VALUES (8, N'Sản phẩm 8 ', N'san-pham-8', N'sp8', N'Sản phẩm 8  Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 ', N'<p>Sản phẩm 8&nbsp; Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8&nbsp; Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8&nbsp; Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8&nbsp; Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8&nbsp; Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8&nbsp; Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8 Sản phẩm 8</p>
', 1, 500000, 0, N'Made in VN', N'37; 38; 39; 40', N'Đỏ; Xanh; Nâu; Xám;Tím; Cam', 1000, 1, 1, 1, '20141211 22:50:17:173')
SET IDENTITY_INSERT dbo.Products OFF


-- `dbo.ProductPhotoes`
SET IDENTITY_INSERT dbo.ProductPhotoes ON

INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (1, N'85_h1_784_390.jpg', N'2-Photo635536738137171473.jpg', 1000, 2)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (2, N'2014-11-17_105010.png', N'1-Photo635536738349128044.png', 1000, 1)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (3, N'dt-huu-tuyen.jpg', N'3-Photo635536739084801274.jpg', 1000, 3)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (4, N'dau-thu-truyen-hinh.jpg', N'4-Photo635536739708613673.jpg', 1000, 4)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (5, N'gia-dung.jpg', N'5-Photo635536740377804158.jpg', 1000, 5)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (6, N'dt-vo-tuyen.jpg', N'6-Photo635539348555378171.jpg', 1000, 6)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (7, N'images.jpg', N'7-Photo635539349437065744.jpg', 1000, 7)
INSERT dbo.ProductPhotoes (Id, Title, FileName, DisplayOrder, ProductId) VALUES (8, N'gia-dung.jpg', N'8-Photo635539350173349028.jpg', 1000, 8)
SET IDENTITY_INSERT dbo.ProductPhotoes OFF


-- `dbo.Users`
SET IDENTITY_INSERT dbo.Users ON

INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (12, N'tuannh', N'lpW0QFLTrnh21pWhoMaEu0FJxFQ=', N'twbtipZH4llNke+XC+A+UQ==', N'tuanh@yahoo.com', N'Tuan Nguyen', 1, 0, '20141217 00:42:55:640', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141212 00:00:00:000', 1, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (14, N'admin2', N'WJ7OshjWK6tEy1P4lQSmj7gUoLQ=', N'L92oPfZGXlWZniGSyQSIVA==', N'nht257@yahoo.com', N'Tuan Nguyen', 1, 0, '20141218 00:11:38:490', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141226 00:00:00:000', 2, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (15, N'admin', N'lpW0QFLTrnh21pWhoMaEu0FJxFQ=', N'twbtipZH4llNke+XC+A+UQ==', N'tuanh@yahoo.com', N'Tuan Nguyen', 1, 0, '20141218 00:19:58:077', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141226 00:00:00:000', 2, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (16, N'admin', N'6wAbL0GxEP74Gn5kNsvm8N3gG0U=', N'3QEB66Lo8g4FScXfPuHFNg==', N'tuanh@yahoo.com', N'Tuan Nguyen', 1, 0, '20141218 00:21:35:147', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141217 00:00:00:000', 1, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (17, N'tuans', N'jsFJMYOJbBsO8I7tO3OZZ+YVmQ8=', N'lPGk8EHiBNxiTz7fFqYqRg==', N'tuan@yahoo.com', N'tuan nguyen', 1, 0, '20141218 00:25:41:797', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141203 00:00:00:000', 1, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (18, N'admin', N'1cXeYpkja2MBHEuCDxAyrOHSoXo=', N'ALNy8fmjtAEiKIQDAF3uVA==', N'tuanh@yahoo.com', N'Tuan Nguyen', 1, 0, '20141218 00:30:21:250', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141224 00:00:00:000', 2, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (19, N'admin5', N'7qFvoGReKUGRm+8DM8vxo/HAP/c=', N'8GbfxXMqqxg56TTW3Lb/vg==', N'nht257@yahoo.com', N'Tuan Nguyen', 1, 0, '20141218 00:35:18:703', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141224 00:00:00:000', 1, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (20, N'admin7', N'1m6dEYLMCKuXsKCdsf1lXELt+ps=', N'jUARy98zmvqVI3h7HTxQ3A==', N'nht257@yahoo.com', N'Tuan Nguyen', 1, 0, '20141218 00:44:26:197', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141226 00:00:00:000', 2, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (21, N'tuannh123', N'iXgLuhGbrTJZAkd3YQmnDykbEQU=', N'ZSpbju3ZhhTGBoyV4PtBLQ==', N'tuannh@cateno.no', N'Nguyên Tấn Tuấn', 1, 0, '20141218 00:46:09:480', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '20141218 00:00:00:000', 1, NULL, NULL)
INSERT dbo.Users (Id, Username, Password, PasswordSalt, Email, FullName, Active, IsAdmin, CreatedDate, UpdatedDate, LastLogin, ResetCode, ResetExpiredCode, Cellphone, Telphone, Address, TotalPoints, Birthday, Gender, DistrictId, ProvinceId) VALUES (22, N'abcdef', N'XJI4vy/KYRHdn1+PAa4ocwIUsCI=', N'/tphCVpjJROMukVZnQuFGg==', N'tuan@bac.com', N'tuan Nguyen', 1, 0, '20141218 00:47:10:327', NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, '19100520 00:00:00:000', 1, NULL, NULL)
SET IDENTITY_INSERT dbo.Users OFF


-- `dbo.Orders`
SET IDENTITY_INSERT dbo.Orders ON

INSERT dbo.Orders (Id, UserId, CustomerName, Phone, Email, Address, DistrictId, Note, OrderDate, ProvinceId, Status) VALUES (3, 12, N'Tuan Nguyen', N'123456789', N'nht257@yahoo.com', N'326 - Điện Biện Phủ - p17 - BT', 26, N'sdfasdfasdfsadf sfsadfsdf', '20141221 19:01:41:930', NULL, 1)
SET IDENTITY_INSERT dbo.Orders OFF


-- `dbo.ProductOrders`
SET IDENTITY_INSERT dbo.ProductOrders ON

INSERT dbo.ProductOrders (Id, ProductId, OrderId, Quatity, Price, [Size], Color) VALUES (5, 1, 3, 1, 300000, NULL, NULL)
INSERT dbo.ProductOrders (Id, ProductId, OrderId, Quatity, Price, [Size], Color) VALUES (6, 7, 3, 1, 500000, NULL, NULL)
INSERT dbo.ProductOrders (Id, ProductId, OrderId, Quatity, Price, [Size], Color) VALUES (7, 5, 3, 1, 400000, NULL, NULL)
SET IDENTITY_INSERT dbo.ProductOrders OFF


-- `dbo.UserPoints`
SET IDENTITY_INSERT dbo.UserPoints ON

INSERT dbo.UserPoints (Id, UserId, OrderId, Points, Note, CreatedDate) VALUES (1, 12, 3, 10, N'Mua hàng', '20141221 00:00:00:000')
SET IDENTITY_INSERT dbo.UserPoints OFF

