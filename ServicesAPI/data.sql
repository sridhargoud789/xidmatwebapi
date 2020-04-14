SET IDENTITY_INSERT [dbo].[Company] ON 
GO
INSERT [dbo].[Company] ([id], [CompanyName], [IsActive], [Description], [CreatedOn], [UpdatedOn], [UpdatedBy], [CountryCode]) VALUES (1, N'Feroz services ', 1, N'Test services ', CAST(N'2020-04-05T22:32:41.460' AS DateTime), NULL, NULL, NULL)
GO
INSERT [dbo].[Company] ([id], [CompanyName], [IsActive], [Description], [CreatedOn], [UpdatedOn], [UpdatedBy], [CountryCode]) VALUES (2, N'My Services 007', 1, N'My Services 007 My Services 007 My Services 007 My Services 007 ', CAST(N'2020-04-06T18:19:11.000' AS DateTime), NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Company] OFF
GO
SET IDENTITY_INSERT [dbo].[CompanyProfileMedia] ON 
GO
INSERT [dbo].[CompanyProfileMedia] ([id], [FileName], [FilePath], [CompanyID], [IsActive], [CreatedOn], [UpdatedOn], [UpdatedBy], [FileId]) VALUES (1, NULL, NULL, 1, 1, CAST(N'2020-04-05T22:32:41.587' AS DateTime), NULL, NULL, 0)
GO
INSERT [dbo].[CompanyProfileMedia] ([id], [FileName], [FilePath], [CompanyID], [IsActive], [CreatedOn], [UpdatedOn], [UpdatedBy], [FileId]) VALUES (2, NULL, NULL, 2, 1, CAST(N'2020-04-06T18:19:11.287' AS DateTime), NULL, NULL, 0)
GO
SET IDENTITY_INSERT [dbo].[CompanyProfileMedia] OFF
GO
SET IDENTITY_INSERT [dbo].[CompanyServices] ON 
GO
INSERT [dbo].[CompanyServices] ([id], [CompanyID], [MasterServiceID], [IsApproved], [ApprovedOn], [ApprovedBy], [IsActive], [CreatedOn], [CreatedBy], [ServiceTitle], [ServiceDescription], [Timings], [CountryCode], [City], [Place], [Lattitude], [Longitude]) VALUES (1, 1, 1, 0, NULL, NULL, 0, CAST(N'2020-04-05T22:33:28.603' AS DateTime), 88, N'Best tour agency ', N'Test test test ', N'24 hrs ', N'AE', N'Sharjah ', NULL, NULL, NULL)
GO
INSERT [dbo].[CompanyServices] ([id], [CompanyID], [MasterServiceID], [IsApproved], [ApprovedOn], [ApprovedBy], [IsActive], [CreatedOn], [CreatedBy], [ServiceTitle], [ServiceDescription], [Timings], [CountryCode], [City], [Place], [Lattitude], [Longitude]) VALUES (2, 2, 9, 0, NULL, NULL, 0, CAST(N'2020-04-06T18:21:46.693' AS DateTime), 90, N'Cat Care Feeding', N'Cat Care Feeding Cat Care Feeding Cat Care Feeding Cat Care Feeding Cat Care Feeding ', N'any time', N'AE', N'Sharjah', N'Muwailih Commercial - Sharjah - United Arab Emirates', N'25.3009483', N'55.45269949999999')
GO
SET IDENTITY_INSERT [dbo].[CompanyServices] OFF
GO
SET IDENTITY_INSERT [dbo].[MasterServices] ON 
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (1, N'Tour Guide', N'Tour Guide', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (2, N'Transportation', N'Transportation', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (3, N'Cleaning / Nany Services', N'Cleaning / Nany Services', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (4, N'Car Repair / Service Center', N'Car Repair / Service Center', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (5, N'Handy Man', N'Handy Man', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (6, N'Health Care', N'Health Care', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (7, N'Rent A Car', N'Rent A Car', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (8, N'Sport & Fitness', N'Sport & Fitness', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (9, N'Pet Care', N'Pet Care', 1, N'')
GO
INSERT [dbo].[MasterServices] ([id], [Title], [Description], [IsActive], [ImagePath]) VALUES (10, N'Others', N'Others', 1, N'')
GO
SET IDENTITY_INSERT [dbo].[MasterServices] OFF
GO
SET IDENTITY_INSERT [dbo].[MyProducts] ON 
GO
INSERT [dbo].[MyProducts] ([Id], [MasterProductId], [ProductName], [ProductDescription], [UserId], [CreatedOn], [IsActive], [IsApproved], [CustomerViewsCount], [City], [CountryCode], [Place], [Lattitude], [Longitude]) VALUES (1, 6, N'Men''s watches ', N'Got 5 watches to sell, 2 stuhrling watches with a big dial, one blade watch, one nautica and one puma sports watch.

All these watches are authentic and perfectly working, I have used then personally but now plan to sell all of them together so if anyone one who is interested in watch collection then this is it with lowest price possible, the silver sthurling watch has a small scratch on the screen but can be changed for a small amount. WhatsApp me on 0502232183 ', 87, CAST(N'2020-04-04T16:19:13.237' AS DateTime), 1, NULL, NULL, N'Sharjah ', N'AE', NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[MyProducts] OFF
GO
SET IDENTITY_INSERT [dbo].[MyProductsMedia] ON 
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (1, 1, N'5e99341f-95cb-4028-9c7b-94971041e2a2.jpg', NULL, CAST(N'2020-04-04T16:19:13.393' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (2, 1, N'43c60639-7291-465a-94a0-a15cfd9a8c7b.jpg', NULL, CAST(N'2020-04-04T16:19:13.393' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (3, 1, N'4742f691-2bd9-483c-a8f3-9f7af239a6be.jpg', NULL, CAST(N'2020-04-04T16:19:13.393' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (4, 1, N'f6207b01-4a45-458f-8a17-90a8f142d52c.jpg', NULL, CAST(N'2020-04-04T16:19:13.393' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (5, 1, N'e9181618-4572-48c7-9b97-2dd1740fbff3.jpg', NULL, CAST(N'2020-04-04T16:19:13.397' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (6, 1, N'b06b0bf5-011f-40d2-89e5-e2ec3623dfc3.jpg', NULL, CAST(N'2020-04-04T16:19:13.397' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (7, 2, N'd28b1c7a-d231-4f77-bafb-fbc604606b2f.png', NULL, CAST(N'2020-04-06T15:57:54.917' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (8, 3, N'a2d6bed1-d5d0-407f-97e4-d1d091bbbbc7.jpg', NULL, CAST(N'2020-04-06T17:32:03.910' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (9, 3, N'bae0ab0e-8bbf-4805-a39a-caf797421d8a.jpg', NULL, CAST(N'2020-04-06T17:32:03.910' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (10, 3, N'57bb64ab-3cd6-48e0-8e38-c8613fa00d02.jpg', NULL, CAST(N'2020-04-06T17:32:03.910' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (11, 3, N'8c37a207-8e61-47f5-ac46-c061f569e25d.jpg', NULL, CAST(N'2020-04-06T17:32:03.910' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (12, 3, N'6372effe-d9c3-4855-bc0d-c44c84eec2df.jpg', NULL, CAST(N'2020-04-06T17:32:03.910' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (13, 3, N'd87024d6-2d15-4c72-8144-9a57e66d4e22.jpg', NULL, CAST(N'2020-04-06T17:32:03.910' AS DateTime), 1)
GO
INSERT [dbo].[MyProductsMedia] ([Id], [MyProductId], [FileId], [CreatedBy], [CreatedOn], [IsActive]) VALUES (14, 4, N'a9c3fcfb-546c-4d2c-9157-db40d657a67c.jpeg', NULL, CAST(N'2020-04-06T19:04:05.997' AS DateTime), 1)
GO
SET IDENTITY_INSERT [dbo].[MyProductsMedia] OFF
GO
INSERT [dbo].[ProductCategories] ([Id], [ProductName]) VALUES (1, N'Furniture')
GO
INSERT [dbo].[ProductCategories] ([Id], [ProductName]) VALUES (2, N'Electronics')
GO
INSERT [dbo].[ProductCategories] ([Id], [ProductName]) VALUES (3, N'Baby Items')
GO
INSERT [dbo].[ProductCategories] ([Id], [ProductName]) VALUES (4, N'Cars')
GO
INSERT [dbo].[ProductCategories] ([Id], [ProductName]) VALUES (5, N'Property for Sale / Rent')
GO
INSERT [dbo].[ProductCategories] ([Id], [ProductName]) VALUES (6, N'Others')
GO
SET IDENTITY_INSERT [dbo].[ServicesMedia] ON 
GO
INSERT [dbo].[ServicesMedia] ([id], [FileName], [FilePath], [ServicesID], [IsActive], [CreatedOn], [UpdatedOn], [UpdatedBy], [FileId]) VALUES (1, NULL, NULL, 2, 1, CAST(N'2020-04-06T18:21:46.943' AS DateTime), NULL, NULL, N'dfe55efe-a904-4b8a-9d55-cd70537d7df3.jpg')
GO
INSERT [dbo].[ServicesMedia] ([id], [FileName], [FilePath], [ServicesID], [IsActive], [CreatedOn], [UpdatedOn], [UpdatedBy], [FileId]) VALUES (2, NULL, NULL, 2, 1, CAST(N'2020-04-06T18:21:46.943' AS DateTime), NULL, NULL, N'40a44807-2bd9-4e9b-bc73-9d7f3daf0503.jpg')
GO
SET IDENTITY_INSERT [dbo].[ServicesMedia] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserId], [EmailId], [Password], [PasswordSalt], [FirstName], [LastName], [Gender], [DOB], [MobileNoCountryCode], [MobileNo], [PhoneNoCountryCode], [PhoneNo], [CreatedOn], [CreatedBy], [IsActive], [CompanyID], [UpdatedOn], [RoleId], [IsApproved], [IsForgotPassword], [ForgotPasswordUID]) VALUES (1, N'sridhargoud789@gmail.com', N'34-44-D5-AD-36-07-E0-DB-FD-63-51-2D-4B-3B-06-1D', N'vzR10aG6Tb', N'Sridhar Goud', N'Siddagowni', N'Male', CAST(N'1988-05-08' AS Date), N'+971', N'566285608', N'+971', N'44384247', CAST(N'2020-02-16T12:51:57.607' AS DateTime), NULL, 1, 0, NULL, 1, 1, 1, N'793343')
GO
INSERT [dbo].[Users] ([UserId], [EmailId], [Password], [PasswordSalt], [FirstName], [LastName], [Gender], [DOB], [MobileNoCountryCode], [MobileNo], [PhoneNoCountryCode], [PhoneNo], [CreatedOn], [CreatedBy], [IsActive], [CompanyID], [UpdatedOn], [RoleId], [IsApproved], [IsForgotPassword], [ForgotPasswordUID]) VALUES (8, N'feroz@xidmat.com', N'EF-DB-A2-76-70-77-F5-C2-E2-A9-25-4A-72-D7-61-76', N'Z9q6dPSjM2', N'Feroz', N'Khan', N'Male', NULL, NULL, N'0566666666', NULL, N'04777888', CAST(N'2020-03-17T22:36:38.313' AS DateTime), NULL, 1, 0, NULL, 1, 1, 1, N'199389')
GO
INSERT [dbo].[Users] ([UserId], [EmailId], [Password], [PasswordSalt], [FirstName], [LastName], [Gender], [DOB], [MobileNoCountryCode], [MobileNo], [PhoneNoCountryCode], [PhoneNo], [CreatedOn], [CreatedBy], [IsActive], [CompanyID], [UpdatedOn], [RoleId], [IsApproved], [IsForgotPassword], [ForgotPasswordUID]) VALUES (87, N'm.feroz@eim.ae', N'6C-86-C9-FC-F9-FF-50-80-BC-BF-30-91-79-0E-02-78', N'HDWT3oXosX', N'Muhammad ', N'Khan ', N'Male', NULL, NULL, N'0502232183', NULL, N'0502232183', CAST(N'2020-04-04T16:17:17.993' AS DateTime), NULL, 1, 0, NULL, 3, 1, NULL, NULL)
GO
INSERT [dbo].[Users] ([UserId], [EmailId], [Password], [PasswordSalt], [FirstName], [LastName], [Gender], [DOB], [MobileNoCountryCode], [MobileNo], [PhoneNoCountryCode], [PhoneNo], [CreatedOn], [CreatedBy], [IsActive], [CompanyID], [UpdatedOn], [RoleId], [IsApproved], [IsForgotPassword], [ForgotPasswordUID]) VALUES (88, N'feroz999@gmail.com', N'E3-BF-86-4C-C5-D9-5C-AC-FD-DB-E4-37-D2-23-37-5C', N'u3XNzKX21T', N'Feroz', N'Khan', N'Male', NULL, NULL, N'0502232183', NULL, N'0502232183', CAST(N'2020-04-05T22:32:41.710' AS DateTime), NULL, 0, 1, NULL, 2, 0, NULL, NULL)
GO
INSERT [dbo].[Users] ([UserId], [EmailId], [Password], [PasswordSalt], [FirstName], [LastName], [Gender], [DOB], [MobileNoCountryCode], [MobileNo], [PhoneNoCountryCode], [PhoneNo], [CreatedOn], [CreatedBy], [IsActive], [CompanyID], [UpdatedOn], [RoleId], [IsApproved], [IsForgotPassword], [ForgotPasswordUID]) VALUES (89, N'myproduct007@gmail.com', N'78-79-63-C0-14-A4-8C-9B-03-3B-44-CD-D3-A3-F7-4F', N'41Sq27NJNS', N'My', N'Product', N'Male', NULL, NULL, N'056757657', NULL, N'04546548', CAST(N'2020-04-06T08:55:02.517' AS DateTime), NULL, 0, 0, NULL, 3, 0, NULL, NULL)
GO
INSERT [dbo].[Users] ([UserId], [EmailId], [Password], [PasswordSalt], [FirstName], [LastName], [Gender], [DOB], [MobileNoCountryCode], [MobileNo], [PhoneNoCountryCode], [PhoneNo], [CreatedOn], [CreatedBy], [IsActive], [CompanyID], [UpdatedOn], [RoleId], [IsApproved], [IsForgotPassword], [ForgotPasswordUID]) VALUES (90, N'myservice007@gmail.com', N'18-25-1F-86-99-40-44-CE-0B-F8-2B-EF-30-38-F9-71', N'29BTdUe5Om', N'MY', N'Service', N'Male', NULL, NULL, N'056757657', NULL, N'046546546', CAST(N'2020-04-06T18:19:11.520' AS DateTime), NULL, 1, 2, NULL, 2, 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
