
CREATE DATABASE ConstructionEquipmentRentalDB;
GO

USE ConstructionEquipmentRentalDB;
GO


-- Tạo bảng Account
CREATE TABLE Account (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StoreId INT NULL, -- Cho phép NULL
    Username NVARCHAR(255) NOT NULL,
	FullName NVARCHAR(255) NOT NULL,
	Gender NVARCHAR(50) DEFAULT 'OTHER' NOT NULL, 
    Password NVARCHAR(MAX) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Phone NVARCHAR(20),
    Address NVARCHAR(MAX),
    DateOfBirth DATE,
    Picture NVARCHAR(MAX),
    GoogleId NVARCHAR(255),
    Role NVARCHAR(50) DEFAULT 'CUSTOMER' NOT NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL,
    Points INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE() NOT NULL
);

-- Thêm Unique Filtered Index cho cột StoreId
CREATE UNIQUE INDEX IX_Account_StoreId ON Account(StoreId) WHERE StoreId IS NOT NULL;

-- Tạo bảng Store
CREATE TABLE Store (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Address NVARCHAR(MAX) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL,
    OpeningDay DATE NOT NULL,
    OpeningHours TIME NOT NULL,
    ClosingHours TIME NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    BusinessLicense NVARCHAR(MAX),
    AccountId INT UNIQUE,
    FOREIGN KEY (AccountId) REFERENCES Account(Id)
);



-- Tạo bảng Brand
CREATE TABLE Brand (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Country NVARCHAR(255) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL
);

-- Tạo bảng Category
CREATE TABLE Category (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX)NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL
);
ALTER TABLE Category
ADD ImageUrl NVARCHAR(MAX) NULL;


-- Tạo bảng Product
CREATE TABLE Product (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Category(Id),
    BrandId INT NOT NULL FOREIGN KEY REFERENCES Brand(Id),
    StoreId INT NOT NULL FOREIGN KEY REFERENCES Store(Id),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    DefaultImage NVARCHAR(MAX) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    Discount DECIMAL(18,2) ,
    PriceSale DECIMAL(18,2) ,
    Stock INT NOT NULL,
    Status NVARCHAR(50) DEFAULT 'AVAILABLE' NOT NULL,
    DiscountStartDate DATETIME,
    DiscountEndDate DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    Weight DECIMAL(10,2), -- Trọng lượng (kg)
    Dimensions NVARCHAR(100), -- Kích thước (Dài x Rộng x Cao)
    FuelType NVARCHAR(50) -- Loại nhiên liệu (Diesel, xăng, điện, ...)
   

);

-- Tạo bảng ProductImage
CREATE TABLE ProductImage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    ImageUrl NVARCHAR(MAX) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL
);

CREATE TABLE Cart (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountId INT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE() NOT NULL,
	Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES Account(Id)
);

CREATE TABLE CartItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    TotalPrice AS (Quantity * Price) PERSISTED,
    FOREIGN KEY (CartId) REFERENCES Cart(Id),
    FOREIGN KEY (ProductId) REFERENCES Product(Id)
);


-- Tạo bảng Order
CREATE TABLE [Order] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    Status NVARCHAR(50) DEFAULT 'PENDING' NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) DEFAULT 'CASH' NOT NULL,
    PurchaseMethod NVARCHAR(50) DEFAULT 'ONLINE' NOT NULL,
    RecipientName NVARCHAR(255) NOT NULL,
    RecipientPhone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    DateOfReceipt DATE NOT NULL,
    DateOfReturn DATE NOT NULL
);
ALTER TABLE [Order]
ADD PayOsUrl NVARCHAR(MAX) NULL;
ALTER TABLE [Order]
ADD UpdatedAt DATETIME DEFAULT GETDATE() NULL;

-- Tạo bảng OrderItem
CREATE TABLE OrderItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
	TotalPrice DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL
);

-- Tạo bảng Feedback
CREATE TABLE Feedback (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    Rating INT CHECK (Rating BETWEEN 1 AND 5) NOT NULL,
    Comment NVARCHAR(MAX),
	HideName BIT DEFAULT 0 NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    UpdatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    Status NVARCHAR(50) DEFAULT 'PENDING' NOT NULL
);

-- Tạo bảng OrderReport
CREATE TABLE OrderReport (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    ReporterId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    HandlerId INT NULL FOREIGN KEY REFERENCES Account(Id),
    Reason NVARCHAR(MAX) NOT NULL,
    Details NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'PENDING' NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    ResolvedAt DATETIME
);

-- Bảng Transaction
CREATE TABLE [Transaction] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT UNIQUE, -- Một giao dịch chỉ liên kết với một đơn hàng
    AccountId INT NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL,
    TotalPrice DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) DEFAULT 'PENDING' NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES [Order](Id),
    FOREIGN KEY (AccountId) REFERENCES Account(Id)
);



-- Bảng Wallet
CREATE TABLE Wallet (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountId INT NOT NULL UNIQUE,
    BankName NVARCHAR(255),
    BankAccount NVARCHAR(255),
    Balance DECIMAL(18,2) DEFAULT 0.00 NOT NULL,
    Status NVARCHAR(50) DEFAULT 'ACTIVE' NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    FOREIGN KEY (AccountId) REFERENCES Account(Id)
);

-- Bảng WalletLog
CREATE TABLE WalletLog (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    WalletId INT NOT NULL,
    TransactionId INT UNIQUE, 
    Type NVARCHAR(50) NOT NULL CHECK (Type IN ('ADD', 'SUBTRACT')),
    Amount DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE() NOT NULL,
    Status NVARCHAR(50) DEFAULT 'COMPLETED' NOT NULL,
    FOREIGN KEY (WalletId) REFERENCES Wallet(Id),
    FOREIGN KEY (TransactionId) REFERENCES [Transaction](Id)
);

-- Tạo bảng RefreshToken
CREATE TABLE RefreshToken (
    RefreshTokenID INT PRIMARY KEY IDENTITY(1,1),
    ExpiredAt DATETIME NOT NULL,
    Token VARCHAR(255) NOT NULL,
    AccountID INT NOT NULL FOREIGN KEY REFERENCES Account(Id)
);



-- Thêm dữ liệu vào bảng Account
INSERT INTO Account (StoreId, Username, FullName, Gender, Password, Email, Phone, Address, DateOfBirth, Picture, GoogleId, Role, Status)
VALUES 
(1, 'storeowner1', 'John Doe', 'MALE', 'hashed_password1', 'owner1@store.com', '1234567890', '123 Main St', '1980-01-01', NULL, NULL, 'LESSOR', 'ACTIVE'),
(2, 'storeowner2', 'John Doe', 'MALE', 'hashed_password2', 'owner2@store.com', '0987654321', '456 Market Ave', '1985-02-15', NULL, NULL, 'LESSOR', 'ACTIVE'),
(3, 'customer1', 'John Doe', 'MALE', 'hashed_password3', 'customer1@gmail.com', '1122334455', '789 Elm St', '1995-06-10', NULL, NULL, 'CUSTOMER', 'ACTIVE'),
(4, 'customer2', 'John Doe', 'MALE', 'hashed_password4', 'customer2@gmail.com', '6677889900', '321 Oak St', '1992-03-22', NULL, NULL, 'CUSTOMER', 'ACTIVE'),
(5, 'admin1', 'John Doe', 'MALE', 'hashed_password5', 'admin@system.com', '5544332211', 'Admin Office', '1975-12-05', NULL, NULL, 'ADMIN', 'ACTIVE'),
(6, 'duylessor', 'luong ngoc phuong duy', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'tonyluong1910@gmail.com', '0399191045', 'tp thu duc', '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'LESSOR', 'ACTIVE'),
(NULL, 'khangdoan1811', 'Khang Đoàn', 'MALE', '4f2076862993d3eebbe9c5eec4e4fd1a5d3e1b6913bade5c7d1d977c2262d22c', 'Khangdase171370@fpt.edu.vn', '0374277590', '2555 Bình trường', '2003-11-11', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'STAFF', 'ACTIVE'),
(NULL, 'datcustomer', 'tuan dat customer', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'lduyyy1910@gmail.com', '0399191045', 'quan binh thanh', '2003-10-10', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'tonylg', 'phương duy', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'iamtonyluong@gmail.com', '0399191045', 'tp thủ đức', '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'testuser', 'Tuấn Đạt', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'duyluongg19100@gmail.com', '0399191045', 'sài gòn', '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'dattest', 'tuan dat', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'duyluongg1910@gmail.com', '0399191045', 'saigon', '2003-10-10', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'sampleuser12', 'Sample User 12', 'MALE', 'samplepassword12', 'sample12@email.com', '0123456789', 'Sample Address 12', '1980-01-02', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=sampletoken12', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'sampleuser13', 'Sample User 13', 'MALE', 'samplepassword13', 'sample13@email.com', '0123456790', 'Sample Address 13', '1980-01-03', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=sampletoken13', NULL, 'CUSTOMER', 'ACTIVE'),
(7, 'TuanDatLessor', 'Tuan Dat Coder', 'MALE', '4f3a273bfe1a40a27657560e3632df6924e4af935124d863b5a5f9fde7338a38', 'tdpride89@gmail.com', '0925778123', 'strin1123g', '2024-01-15', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'LESSOR', 'ACTIVE'),
(NULL, 'khangdepzai', 'khangdoan', 'MALE', '4f2076862993d3eebbe9c5eec4e4fd1a5d3e1b6913bade5c7d1d977c2262d22c', 'sparkventuresfpt@gmail.com', '037427', '33', '2003-11-11', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'Tu', 'Nguyen', 'MALE', 'd139a0adb5a7ee7daef19d8df45b19fbe476bef273955379da31b8aef04af08f', 'tunguyen100312@gmail.com', '0792628243', 'Thu Duc', '2003-12-10', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(8, 'TDLessor2', 'Tuan Dat Coder', 'MALE', '4f3a273bfe1a40a27657560e3632df6924e4af935124d863b5a5f9fde7338a38', 'dinhhoangdat789@gmail.com', '0925778789', 'strin1123g', '2024-01-11', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'LESSOR', 'ACTIVE'),
(NULL, 'TuanDatAdmin', 'Tuan Dat Coder', 'MALE', '4f3a273bfe1a40a27657560e3632df6924e4af935124d863b5a5f9fde7338a38', 'tuandatdq03@gmail.com', '0925778123', 'strin1123g', '2024-01-11', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'ADMIN', 'ACTIVE'),
(NULL, 'khangdoan123', 'Đoàn Anh khang', 'MALE', '4f2076862993d3eebbe9c5eec4e4fd1a5d3e1b6913bade5c7d1d977c2262d22c', 'tesstingcustomer@gmail.com', '0374277590', '255 ấp bình An', '2003-11-11', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(12, 'tonylessor', 'tony luong', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'tonyipad1910@gmail.com', '0399191045', 'tp thủ đức', '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'LESSOR', 'ACTIVE'),
(NULL, 'sampleuser21', 'Sample User 21', 'MALE', 'samplepassword21', 'sample21@email.com', '0123456781', 'Sample Address 21', '1980-01-04', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=sampletoken21', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'tonyadmin', 'Tony Admin', 'MALE', '9d1f559679e78e483d08b65f74bac7cefd290f7ee996914eee613e801941996a', 'tonyluong19102k3@gmail.com', '0399191045', 'saigon', '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'ADMIN', 'ACTIVE'),
(NULL, 'CoGioiPhuongNam', 'Trương Văn Nam', 'MALE', '4f3a273bfe1a40a27657560e3632df6924e4af935124d863b5a5f9fde7338a38', 'cogioiphuongnam@gmail.com', '0908049537', '28 QL1A, Bình An, Dĩ An, Bình Dương, Việt Nam', '1990-01-15', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(13, 'TuanDatSparktech', 'Đinh Quốc Tuấn Đạt', 'MALE', '4f3a273bfe1a40a27657560e3632df6924e4af935124d863b5a5f9fde7338a38', 'sparktechventuresfu@gmail.com', '0922778789', '7 Đ. D1, Long Thạnh Mỹ, Thủ Đức, Hồ Chí Minh', '1991-01-26', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'LESSOR', 'ACTIVE'),
(NULL, 'TuanDat', 'Đinh Quốc Tuấn Đạt', 'MALE', '4f3a273bfe1a40a27657560e3632df6924e4af935124d863b5a5f9fde7338a38', 'datdqtse171685@fpt.edu.vn', NULL, NULL, '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'xaydunganhlong', 'TNHH Anh Long', 'MALE', '30111cf82a1554ec1b6f352b068384483dc3f82fe37ec9ff556e4b191b543ec5', 'xaydunganhlong@gmail.com', '0911887327', '205 Hung Vuong Vo Xu Duc Linh Binh Thuan', '2003-10-19', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=6f3a8425-e611-4f17-b690-08fd7b465219', NULL, 'LESSOR', 'ACTIVE'),
(NULL, 'sampleuser24', 'Sample User 24', 'MALE', 'samplepassword24', 'sample24@email.com', '0123456784', 'Sample Address 24', '1980-01-05', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=sampletoken24', NULL, 'CUSTOMER', 'ACTIVE'),
(NULL, 'sampleuser26', 'Sample User 26', 'MALE', 'samplepassword26', 'sample26@email.com', '0123456786', 'Sample Address 26', '1980-01-06', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/male.png?alt=media&token=sampletoken26', NULL, 'CUSTOMER', 'ACTIVE');

-- Thêm dữ liệu vào bảng Store
INSERT INTO Store (Name, Address, Phone, Status, OpeningDay, OpeningHours, ClosingHours, BusinessLicense, AccountId)
VALUES 
('Store 1', '123 Main St', '1234567890', 'ACTIVE', '2010-05-20', '08:00:00', '18:00:00', 'BL12345', 1),
('Store 2', '456 Market Ave', '0987654321', 'ACTIVE', '2012-07-15', '09:00:00', '19:00:00', 'BL67890', 2),
('Store 3', '789 Central Blvd', '2233445566', 'ACTIVE', '2015-03-10', '07:30:00', '17:30:00', 'BL11223', 3),
('Store 4', '321 Elm St', '4455667788', 'ACTIVE', '2018-11-01', '08:00:00', '18:30:00', 'BL44567', 4),
('Store 5', '987 Park Ln', '6677889900', 'ACTIVE', '2020-09-25', '09:30:00', '20:00:00', 'BL99876', 5),
('Xúc Store', 'Tp Thủ Đức', '0399191045', 'ACTIVE', '2025-03-07', '03:00:00', '16:00:00', 'abcxyz111222-000', 6),
('TuanDat Company', 'Dat', '0925778789', 'ACTIVE', '2025-03-01', '07:30:00', '22:30:00', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/GiayPhepKinhDoanh.jpg?alt=media&token=c1fc0572-1208-40cb-9c73-8620736b4e87', 14),
('TuanVy Company', 'Phạm Văn Đồng, Thủ Đức, TP.HCM', '0938119484', 'ACTIVE', '2025-03-13', '08:00:00', '18:00:00', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/dang-ky-cap-giay-phep-kinh-doanh-cua-hang-nam-viet-luat-1.jpg?alt=media&token=7688063b-2a4e-4c2e-9907-db1b4b7376b4', 17),
('Sample Store 9', 'Sample Address 9', '0123456789', 'ACTIVE', '2020-01-01', '08:00:00', '18:00:00', 'SAMPLE-LICENSE-9', 9),
('Sample Store 10', 'Sample Address 10', '0123456790', 'ACTIVE', '2020-01-02', '09:00:00', '19:00:00', 'SAMPLE-LICENSE-10', 10),
('Sample Store 11', 'Sample Address 11', '0123456791', 'ACTIVE', '2020-01-03', '07:30:00', '17:30:00', 'SAMPLE-LICENSE-12', 11),
('tony xúc ', 'Tp Thủ Đức', '0399191045', 'ACTIVE', '2025-03-25', '13:33:00', '06:33:00', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/Giay%20phep%20kinh%20doanh.jpg?alt=media&token=0401206e-e72a-43b9-a133-b9897a819d77', 20),
('SparkTech Ventures Company', '7 Đ. D1, Long Thạnh Mỹ, Thủ Đức, Hồ Chí Minh', '0922778789', 'ACTIVE', '2025-03-14', '11:00:00', '22:00:00', 'https://storage.googleapis.com/marinepath-56521.appspot.com/e1f95d33-a9d8-48aa-ad74-c81e8193af83.jpg?X-Goog-Algorithm=GOOG4-RSA-SHA256&X-Goog-Credential=marinepath-56521%40appspot.gserviceaccount.com%2F20250326%2Fauto%2Fstorage%2Fgoog4_request&X-Goog-Date=20250326T065416Z&X-Goog-Expires=604800&X-Goog-SignedHeaders=host&X-Goog-Signature=933e937b5e1c9458b8af590f4268cb29177b3492dcf1636c2a5dd503e62ba5bd053af4bfdeab5c839fc6ad1b31bfdc5286607af969d077ef0b23c5451e64a8d7f5d9f0e6acaa32804fbbd503b869bc37c6ad4afc1b3acbc115a34e34384b3c9c52f09e786c7d7e2b82cdd57ed52b471cbdd71d5cf534ec8c6e37a303e9f3e6d1981e4dcb2b2fb431c171cbaee19ee49e594014f150200617afb23aec40d5668015812aca6cdf4688fde5803b400709867c23536e30830d488836bac387df0007964e9eabe8c0a6d62bb3322f6221543a3d57796bb696e8435c6bd02c623c4ea8f37e89bcc76914a9f389139c316c8fa6a7b4f02d41bc34f40cd2fe96d4f8ab33', 25);

-- Thêm dữ liệu vào bảng Brand
INSERT INTO Brand (Name, Description, Country, Status)
VALUES 
('Caterpillar', 'Leading manufacturer of construction equipment.', 'USA', 'ACTIVE'),
('Komatsu', 'World-renowned producer of construction machinery.', 'Japan', 'ACTIVE'),
('Hitachi', 'Specializes in heavy equipment for construction.', 'Japan', 'ACTIVE'),
('Volvo', 'Offers innovative construction machinery solutions.', 'Sweden', 'ACTIVE'),
('JCB', 'Pioneer in construction equipment manufacturing.', 'UK', 'ACTIVE'),
('Samsung', 'Samsung has a long history of manufacturing machinery.', 'Korea', 'ACTIVE'),
('Hyundai', 'Hyundai has a long history of manufacturing machinery', 'Korea', 'ACTIVE'),
('Thaco', 'Thaco has a long history of manufacturing machinery', 'VietNam', 'ACTIVE');

-- Thêm dữ liệu vào bảng Category
INSERT INTO Category (Name, Description, Status, ImageUrl)
VALUES 
('Máy Ủi', 'Heavy machinery for digging and moving earth.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/excavator%20(5).png?alt=media&token=3059b0cb-faf3-4dc8-b40e-4db7ca0d591c'),
('Cần Cẩu', 'Equipment for lifting and transporting materials.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/crane.png?alt=media&token=a2eeaff0-17cb-4ec0-b1ba-210d90a3bb99'),
('Máy Xúc', 'Heavy machinery for digging and excavation.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/excavator%20(2).png?alt=media&token=a68802de-fca1-481c-916f-7bf4f9cf1efe'),
('Máy Cắt', 'Machinery for pushing large amounts of material.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/saw-blade.png?alt=media&token=d8aa936e-68ed-408e-a19c-b8b221203a6d'),
('Máy Hàn', 'Equipment for mixing concrete on site.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/welding.png?alt=media&token=33ba378f-0db5-4449-b7eb-db957b207c2e'),
('Máy Khoan', 'Equipment for drilling into various surfaces.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/drill.png?alt=media&token=0291bf1d-30b9-49aa-af87-88527dd3994c'),
('Xe Lu', 'Machinery for compacting surfaces.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/road-roller.png?alt=media&token=194fb86d-afba-4d23-9f82-481ec0d86ded'),
('Giàn Giáo', 'Temporary structures for construction support.', 'ACTIVE', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/icons8-scaffolding-64.png?alt=media&token=84a9d463-aec4-4dda-b1fa-fe7281a8d1cf');

-- Thêm dữ liệu vào bảng Product
INSERT INTO Product (CategoryId, BrandId, StoreId, Name, Description, DefaultImage, Price, Discount, PriceSale, Stock, Status, Weight, Dimensions, FuelType)
VALUES 
(1, 1, 1, 'CAT 320 Excavator', 'High-performance excavator for tough jobs.', 'cat320.jpg', 120000.00, NULL, NULL, 5, 'AVAILABLE', 22.5, '10x3x3 m', 'DIESEL'),
(2, 2, 1, 'Komatsu WA380 Loader', 'Versatile loader for various construction tasks.', 'wa380.jpg', 95000.00, NULL, NULL, 3, 'AVAILABLE', 18.0, '9x3x3 m', 'DIESEL'),
(3, 3, 2, 'Hitachi ZX200 Excavator', 'Efficient excavator with advanced hydraulics.', 'zx200.jpg', 115000.00, NULL, NULL, 4, 'AVAILABLE', 21.0, '10x3x3 m', 'DIESEL'),
(4, 4, 2, 'Volvo L110 Loader', 'Reliable loader with excellent fuel efficiency.', 'l110.jpg', 105000.00, NULL, NULL, 2, 'AVAILABLE', 20.0, '9x3x3 m', 'DIESEL'),
(5, 5, 3, 'JCB 3CX Backhoe Loader', 'Multi-purpose machine for construction sites.', '3cx.jpg', 75000.00, NULL, NULL, 6, 'AVAILABLE', 8.5, '6x2.5x2.5 m', 'DIESEL'),
(5, 5, 5, 'Máy trộn bê tông 200 Lít', '<p>Di chuyển bằng bánh xe, tiếng ồn nhỏ</p>', 'https://dailoi.vn/uploads/images/2021/09/1631372749-single_product1-maytronbetong200la.jpg.webp', 100000.00, NULL, NULL, 100, 'ACTIVE', 200.00, '20x20x50', 'PETROL'),
(3, 5, 6, 'Máy Xúc Hyundai 55W', '<p>Máy xúc bánh lốp Hyundai 55W với động cơ mạnh mẽ, phù hợp cho công trình vừa và nhỏ.</p>', 'https://timmaymoc.com/upload/img/products/07012022/c7f06eaa7e3263a7b3e7ec5d7e3dcdf7.jpg', 50000.00, NULL, NULL, 20, 'ACTIVE', 2000.00, '20x20x10', 'PETROL'),
(1, 2, 6, 'Máy Ủi Komatsu D85EX', '<p>Máy ủi công suất lớn giúp san lấp mặt bằng nhanh chóng và hiệu quả.</p>', 'https://www.changlin-dao.com/public/media//komatsu-d85e-15-2.jpg', 20000.00, NULL, NULL, 12, 'ACTIVE', 1000.00, '20x20x20', 'DIESEL'),
(8, 3, 6, 'Giàn Giáo 1m7', '<p>Giàn giáo thép 1m7 chắc chắn, giúp đảm bảo an toàn trong xây dựng.</p>', 'https://haiminhjsc.vn/wp-content/uploads/2023/01/gian-giao-khung3.jpg', 25000.00, NULL, NULL, 13, 'ACTIVE', 11000.00, '10x20x30', 'HYBRID'),
(7, 3, 6, 'Xe Lu Rung Sakai SV520D', '<p>Xe lu rung Sakai giúp nén chặt đất nền, tạo độ bền cho công trình.</p>', 'https://bizweb.dktcdn.net/100/104/326/products/z4093691418768-a19126eb24676face965c1f7c699c87b-5efd81d8-7ce8-4089-8900-ee4625e3223a.jpg?v=1687498181817', 15000.00, NULL, NULL, 27, 'ACTIVE', 3000.00, '10x20x50', 'HYBRID'),
(3, 3, 7, 'MÁY ĐÀO BÁNH XÍCH HITACHI ZX200-5G', '<p><span style="color: rgb(64, 64, 64);">Máy đào bánh xích Hitachi ZX200-5G là dòng máy đào cỡ trung, phù hợp cho các công trình xây dựng, khai thác mỏ và các dự án hạ tầng. Máy được trang bị động cơ mạnh mẽ, hệ thống thủy lực tiên tiến, và cabin tiện nghi, đảm bảo hiệu suất làm việc cao và thoải mái cho người vận hành.</span></p>', 'https://mdx-maycongtrinh.com/wp-content/uploads/2023/05/may-xuc-dao-hitachi-zx200-5g-04.jpeg', 200000.00, NULL, NULL, 100, 'ACTIVE', 20000.00, '~8,500 x 2,800 x 3,000 (mm)', 'DIESEL'),
(3, 8, 7, 'MÁY XÚC ĐÀO HYUNDAI R180W-9S', '<p><span style="color: rgb(64, 64, 64);">Máy xúc đào Hyundai R180W-9S là dòng máy đào bánh lốp cỡ trung, phù hợp cho các công trình xây dựng, hạ tầng đô thị và các dự án yêu cầu tính cơ động cao. Máy được trang bị động cơ mạnh mẽ, hệ thống thủy lực tiên tiến, và cabin tiện nghi, đảm bảo hiệu suất làm việc cao và thoải mái cho người vận hành.</span></p>', 'https://hd-hyundai.vn/public/uploads/8f3ae07f338c05a6b8a279eed5d6dc3d/files/bab5da43-3764-4b53-96f7-9bd58d8fef96.jpg', 200000.00, NULL, NULL, 150, 'ACTIVE', 17000.00, '~8,000 x 2,500 x 3,000 (mm)', 'DIESEL'),
(1, 6, 7, 'MÁY ĐÀO BÁNH XÍCH SAMSUNG SL 120-2', '<p><span style="color: rgb(64, 64, 64);">Máy đào bánh xích Samsung SL 120-2 là dòng máy đào cỡ nhỏ, phù hợp cho các công trình xây dựng, đào đắp trong phạm vi hẹp và không gian làm việc nhỏ. Máy được thiết kế với độ bền cao, tiết kiệm nhiên liệu và dễ dàng bảo trì, bảo dưỡng.</span></p>', 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/1.jpg?alt=media&token=d8fe0b9f-f9a5-4407-8018-79fc974c0851', 123000.00, NULL, NULL, 121, 'ACTIVE', 12000.00, '~6,000 x 2,200 x 2,500 (mm)', 'DIESEL'),
(3, 2, 7, 'MÁY XÚC ĐÀO BÁNH XÍCH KOMATSU PC200-8N1', '<p><span style="color: rgb(33, 37, 41);">Máy đào bánh xích Komatsu PC200-8N1 với dung tích gàu 0.8 m3 là loại máy cỡ trung bình, phục vụ các công tác đào đắp, phụ trợ trong phạm vi vừa và nhỏ. Máy được bảo trì, bảo dưỡng theo đúng tiêu chuẩn của hãng bảo đảm hoạt động ổn định, liên tục.</span></p>', 'https://vuivinhphuc.com/wp-content/uploads/2023/03/z4189323397832_8c32c17bf684c15aa8a66f5e9ca08eb9.jpg', 180000.00, NULL, NULL, 186, 'ACTIVE', 19500.00, '9425 x 2800 x 3040 (mm)', 'DIESEL'),
(3, 2, 7, 'MÁY XÚC ĐÀO BÁNH XÍCH KOMATSU PC78US-8', '<p class="ql-align-justify">Máy đào Komatsu PC78us-8 là máy cỡ nhỏ, dung tích gầu 0.28 m3, xích cao su cho phép làm việc trên mọi địa hình: nền đât, cát , nền bê tông, … bảo đảm thuận tiện cho khách hàng khi sử dụng.</p><p class="ql-align-justify"> </p><p><br></p>', 'https://kfh.com.vn/wp-content/uploads/2019/12/Komatsu-pc78-us-8.jpg', 100000.00, NULL, NULL, 138, 'ACTIVE', 7390.00, '6200 x 2320 x 2730 (mm)', 'DIESEL'),
(3, 2, 7, 'MÁY XÚC ĐÀO KOMASTU', '<p>Rất tốt</p>', 'https://storage.googleapis.com/marinepath-56521.appspot.com/f133bb7d-f2b2-4374-96e6-66813bef4e18.jpg?X-Goog-Algorithm=GOOG4-RSA-SHA256&X-Goog-Credential=marinepath-56521%40appspot.gserviceaccount.com%2F20250325%2Fauto%2Fstorage%2Fgoog4_request&X-Goog-Date=20250325T161231Z&X-Goog-Expires=604800&X-Goog-SignedHeaders=host&X-Goog-Signature=3f9b4e577932141edeb670dea79acbb8dfda297696c0640d746ccfde21043bcd2642d77a281335940bc932f708e984fdb171f45f06e24ef7228142d650321e141abae0ede1260af18e1688df077864f15d8318c4a4124e6aa1162b21d421914a045091dddb09f0e1d3de3862aa67f1c31b5ee422958a8b20c6a77bfcc3a9a69b8a1435dd7dde90ea96ed27b4cfc835f168717de1ec31d7f0335222cbc7c9d0efd53cac2b17ac6e8a971d4bb90abaa5735db5ab3e0afb90b1640bfa10af1e29cdfa3ef146fe8f939cfac00860ebb1856e5bcd37c1d0448ecff91b83450d32be08431c1bf8c11ff961317fe42b3a78e648ee61ce4537308fc21fea4f104c5033bb', 210000.00, NULL, NULL, 10, 'ACTIVE', 2000.00, '7,500 x 2,800 x 2,900 (mm)', 'DIESEL'),
(3, 2, 13, 'MÁY XÚC ĐÀO BÁNH XÍCH KOMATSU PC200-8N1', '<p><span style="color: rgb(33, 37, 41);">Máy đào bánh xích Komatsu PC200-8N1 với dung tích gàu 0.8 m3 là loại máy cỡ trung bình, phục vụ các công tác đào đắp, phụ trợ trong phạm vi vừa và nhỏ. Máy được bảo trì, bảo dưỡng theo đúng tiêu chuẩn của hãng bảo đảm hoạt động ổn định, liên tục.</span></p>', 'https://res.cloudinary.com/dndg7k5id/image/upload/v1742974068/products/238355c5-e6e9-4f25-8f65-0c47d3e6c314.jpg.jpg', 500000.00, NULL, NULL, 100, 'ACTIVE', 2300.00, '7,500 x 2,800 x 2,900 (mm)', 'DIESEL');

-- Thêm dữ liệu vào bảng ProductImage
INSERT INTO ProductImage (ProductId, ImageUrl, Status)
VALUES 
(1, 'images/cat320_front.jpg', 'ACTIVE'),
(1, 'images/cat320_side.jpg', 'ACTIVE'),
(2, 'images/wa380_front.jpg', 'ACTIVE'),
(3, 'images/zx200_front.jpg', 'ACTIVE'),
(4, 'images/l110_front.jpg', 'ACTIVE'),
(6, 'https://dailoi.vn/uploads/images/2021/09/1631372750-multi_product10-maytronbetong200lb.jpg.webp', 'ACTIVE'),
(6, 'https://dailoi.vn/uploads/images/2021/09/1631372750-multi_product10-maytronbetong200lc.jpg.webp', 'ACTIVE'),
(6, 'https://dailoi.vn/uploads/images/2021/09/1631372750-multi_product10-maytronbetong200ld.jpg.webp', 'ACTIVE'),
(7, 'https://www.truck1.vn/img/xxl/35823/Hyundai-ROBEX-55W-9-EXCAVATOR-SPROWADZONY-Z-FRANCJI-Ba-Lan_35823_6770509237694.jpg', 'ACTIVE'),
(7, 'https://img.linemedia.com/img/s/construction-equipment-wheel-excavator-Hyundai-Robex-55W-9-Wheeled-Excavator-5-5t-2pcs---1729063366979877888_common--24101610174529101500.jpg', 'ACTIVE'),
(7, 'https://www.truck1.vn/img/xxl/42544/Hyundai-Robex-55-W-9-Ba-Lan_42544_7433030508264.jpg', 'ACTIVE'),
(8, 'https://upanh.redeptot.vn/photos/maydaokomatsu-vn/2022/04/D85EX_1.jpg', 'ACTIVE'),
(8, 'https://lh6.googleusercontent.com/proxy/RcxA2riKwQNzFFi7Zle45cPbzn3rfkMoA1Ciy64YV7rRkU1QheIfzoBOh9SRTk-pjmJXCBvTosEGvQCEAa-zHMRPbwF0cy2bbxnV4aIxyU-TEO3gCks35w', 'ACTIVE'),
(9, 'https://vattucongtrinh.vn/wp-content/uploads/2024/05/gian-giao-khung-1m7.jpg', 'ACTIVE'),
(9, 'https://giangiaotphcm.vn/wp-content/uploads/2024/05/gian-giao-1m7-tphcm.jpg', 'ACTIVE'),
(10, 'https://pos.nvncdn.com/db2ee7-48164/art/artCT/20191008_qVmG7vo9it7wMLXCcQpOJkkl.png', 'ACTIVE'),
(10, 'https://vicoma.vn/wp-content/uploads/2021/05/520_594x431.png', 'ACTIVE'),
(11, 'https://mdx-hitachi.com/wp-content/uploads/2019/09/may-dao-banh-xich-hitachi-zx200-5g-08.jpg', 'ACTIVE'),
(11, 'https://maycogioichinhhang.com/wp-content/uploads/2021/07/hitachi-zx200-5g-9-533x400.jpg', 'ACTIVE'),
(11, 'https://vitrac.vn/Data/Sites/1/Product/575/may-xuc-dao-hitachi-zx200-5g-01.jpeg', 'ACTIVE'),
(12, 'https://hd-hyundai.vn/public/uploads/8f3ae07f338c05a6b8a279eed5d6dc3d/files/76adbb44-3939-484a-9489-31067b32722f.jpg', 'ACTIVE'),
(12, 'https://hyundai-ce.vn/wp-content/uploads/2021/09/may-xuc-banh-lop-07-1-scaled.jpg', 'ACTIVE'),
(12, 'https://hd-hyundai.vn/public/uploads/8f3ae07f338c05a6b8a279eed5d6dc3d/files/may-xuc-dao-r180w-9s-gau-0-89m3-3.jpg', 'ACTIVE'),
(12, 'https://hyundaituyenquang.com.vn/wp-content/uploads/2020/08/2020_07_22_08_41_IMG_0424.jpg', 'ACTIVE'),
(13, 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/2.jpg?alt=media&token=3eaf3321-de8f-49d4-bf33-d739cb526e1c', 'ACTIVE'),
(13, 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/4.jpg?alt=media&token=32548e85-45c6-489d-8ab5-098a76196b76', 'ACTIVE'),
(13, 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/5.jpg?alt=media&token=ffa0fe0c-2cef-443a-ad69-f80e420a5e4e', 'ACTIVE'),
(13, 'https://firebasestorage.googleapis.com/v0/b/marinepath-56521.appspot.com/o/6.jpeg?alt=media&token=a2fee7b9-5eb4-4697-b77d-ba6730de2113', 'ACTIVE'),
(14, 'https://kfh.com.vn/wp-content/uploads/2019/12/Komatsu-PC-200-8N1.jpg', 'ACTIVE'),
(14, 'https://kfh.com.vn/wp-content/uploads/2019/12/MAY-XUC-DAO-KOMATSU-PC200-8N1.jpg', 'ACTIVE'),
(14, 'https://kfh.com.vn/wp-content/uploads/2019/12/Komatsu-PC200-8N1.jpg', 'ACTIVE'),
(14, 'https://vuivinhphuc.com/wp-content/uploads/2023/03/z4189323410396_677f8b4b3ea2b9d8c72567bcf392ced6.jpg', 'ACTIVE'),
(15, 'https://kfh.com.vn/wp-content/uploads/2019/12/Komatsu-pc78-us-8.jpg', 'ACTIVE'),
(15, 'https://kfh.com.vn/wp-content/uploads/2019/12/Komatsu-PC78us-8.jpg', 'ACTIVE'),
(15, 'https://storage.googleapis.com/marinepath-56521.appspot.com/d9d49576-af35-4725-88aa-f8fc6cc80529.jpg?X-Goog-Algorithm=GOOG4-RSA-SHA256&X-Goog-Credential=marinepath-56521%40appspot.gserviceaccount.com%2F20250312%2Fauto%2Fstorage%2Fgoog4_request&X-Goog-Date=20250312T100535Z&X-Goog-Expires=604800&X-Goog-SignedHeaders=host&X-Goog-Signature=34de45fabd07acc745b79f31b7dbc5f3f9d84e20c7c1017ba37c6b1025c0709e75c6911385ee9b15fe8b1cc792170d4517055f76fcf2ffa5e37df24456778c67384e13acfff41cf4df2b1f533e9836bdf36529043672aa7d13962b0999f59dcf667aec797df17781295bae12fcff8ac9b8f87f4b799f57d6a1b565f8b77393e64019d4233431296c5ee59064ba56ed4e0044d413443a68756c04d6b2bd676a247ee776a4ce2dca4aa8b016b3459ba8103beac269e2c358e4234806945ad47e216e27ae7042ec55ca06eedf0b4189f200630dd1713cc13c24e8fdb67d615b31711a0332f5744946e8ed5a2ce015467cbb434f1b9f7c7bba90d1296cb0838912af', 'ACTIVE'),
(15, 'https://kfh.com.vn/wp-content/uploads/2019/12/Komatsu-PC78us-6-1.jpg', 'ACTIVE'),
(16, 'https://storage.googleapis.com/marinepath-56521.appspot.com/f133bb7d-f2b2-4374-96e6-66813bef4e18.jpg?X-Goog-Algorithm=GOOG4-RSA-SHA256&X-Goog-Credential=marinepath-56521%40appspot.gserviceaccount.com%2F20250325%2Fauto%2Fstorage%2Fgoog4_request&X-Goog-Date=20250325T161231Z&X-Goog-Expires=604800&X-Goog-SignedHeaders=host&X-Goog-Signature=3f9b4e577932141edeb670dea79acbb8dfda297696c0640d746ccfde21043bcd2642d77a281335940bc932f708e984fdb171f45f06e24ef7228142d650321e141abae0ede1260af18e1688df077864f15d8318c4a4124e6aa1162b21d421914a045091dddb09f0e1d3de3862aa67f1c31b5ee422958a8b20c6a77bfcc3a9a69b8a1435dd7dde90ea96ed27b4cfc835f168717de1ec31d7f0335222cbc7c9d0efd53cac2b17ac6e8a971d4bb90abaa5735db5ab3e0afb90b1640bfa10af1e29cdfa3ef146fe8f939cfac00860ebb1856e5bcd37c1d0448ecff91b83450d32be08431c1bf8c11ff961317fe42b3a78e648ee61ce4537308fc21fea4f104c5033bb', 'ACTIVE'),
(17, 'https://res.cloudinary.com/dndg7k5id/image/upload/v1742974068/products/238355c5-e6e9-4f25-8f65-0c47d3e6c314.jpg.jpg', 'ACTIVE');



------------------------------------------------------------------------------------------
-- Dữ liệu mẫu tự thêm cho các OrderId từ 1 đến 10 (Ghi chú: Đây là dữ liệu do tôi tự tạo)
-- Mục đích: Điền các bản ghi thiếu để đảm bảo thứ tự liên tục
INSERT INTO [Order] (CustomerId, Status, TotalPrice, PaymentMethod, PurchaseMethod, RecipientName, RecipientPhone, Address, CreatedAt, DateOfReceipt, DateOfReturn, PayOsUrl, UpdatedAt) VALUES
(1, 'PENDING', 50000.00, 'CASH', 'ONLINE', 'Sample User 1', '0123456789', 'Sample Address 1', '2025-01-01 10:00:00.000', '2025-01-02', '2025-01-04', NULL, '2025-01-01 10:00:00.000'), -- Order 1
(2, 'PENDING', 75000.00, 'PAYOS', 'ONLINE', 'Sample User 2', '0123456790', 'Sample Address 2', '2025-01-02 11:00:00.000', '2025-01-03', '2025-01-05', 'https://pay.payos.vn/web/sample1', '2025-01-02 11:00:00.000'), -- Order 2
(3, 'COMPLETED', 100000.00, 'TRANSFER', 'ONLINE', 'Sample User 3', '0123456791', 'Sample Address 3', '2025-01-03 12:00:00.000', '2025-01-04', '2025-01-06', NULL, '2025-01-03 12:00:00.000'), -- Order 3
(4, 'CANCELLED', 30000.00, 'CASH', 'ONLINE', 'Sample User 4', '0123456792', 'Sample Address 4', '2025-01-04 13:00:00.000', '2025-01-05', '2025-01-07', 'https://pay.payos.vn/web/sample2', '2025-01-04 13:00:00.000'), -- Order 4
(5, 'PENDING', 20000.00, 'PAYOS', 'ONLINE', 'Sample User 5', '0123456793', 'Sample Address 5', '2025-01-05 14:00:00.000', '2025-01-06', '2025-01-08', NULL, '2025-01-05 14:00:00.000'), -- Order 5
(6, 'COMPLETED', 40000.00, 'CASH', 'ONLINE', 'Sample User 6', '0123456794', 'Sample Address 6', '2025-01-06 15:00:00.000', '2025-01-07', '2025-01-09', 'https://pay.payos.vn/web/sample3', '2025-01-06 15:00:00.000'), -- Order 6
(7, 'PENDING', 60000.00, 'TRANSFER', 'ONLINE', 'Sample User 7', '0123456795', 'Sample Address 7', '2025-01-07 16:00:00.000', '2025-01-08', '2025-01-10', NULL, '2025-01-07 16:00:00.000'), -- Order 7
(8, 'CANCELLED', 25000.00, 'PAYOS', 'ONLINE', 'Sample User 8', '0123456796', 'Sample Address 8', '2025-01-08 17:00:00.000', '2025-01-09', '2025-01-11', 'https://pay.payos.vn/web/sample4', '2025-01-08 17:00:00.000'), -- Order 8
(9, 'COMPLETED', 80000.00, 'CASH', 'ONLINE', 'Sample User 9', '0123456797', 'Sample Address 9', '2025-01-09 18:00:00.000', '2025-01-10', '2025-01-12', NULL, '2025-01-09 18:00:00.000'), -- Order 9
(10, 'PENDING', 35000.00, 'PAYOS', 'ONLINE', 'Sample User 10', '0123456798', 'Sample Address 10', '2025-01-10 19:00:00.000', '2025-01-11', '2025-01-13', 'https://pay.payos.vn/web/sample5', '2025-01-10 19:00:00.000'); -- Order 10

-- Dữ liệu thực tế bạn cung cấp từ OrderId 11 đến 91
INSERT INTO [Order] (CustomerId, Status, TotalPrice, PaymentMethod, PurchaseMethod, RecipientName, RecipientPhone, Address, CreatedAt, DateOfReceipt, DateOfReturn, PayOsUrl, UpdatedAt) VALUES
(8, 'PENDING', 80000.00, 'CASH', 'ONLINE', 'duy', '0399191045', 'tp thủ đức', '2025-03-09 19:25:06.480', '2025-03-11', '2025-03-13', NULL, '2025-03-09 19:25:06.480'), -- Order 11
(8, 'PENDING', 30000.00, 'CASH', 'ONLINE', 'duy', '0399191045', 'quan 2', '2025-03-10 03:38:12.357', '2025-03-11', '2025-03-13', NULL, '2025-03-10 03:38:12.357'), -- Order 12
(8, 'PENDING', 15000.00, 'PAYOS', 'ONLINE', 'duy', '0399191045', 'u cà', '2025-03-11 16:16:58.263', '2025-03-12', '2025-03-13', NULL, '2025-03-11 16:16:58.263'), -- Order 13
(8, 'CANCELLED', 15000.00, 'PAYOS', 'ONLINE', 'duy', '0399191045', 'u cà', '2025-03-11 16:18:13.897', '2025-03-12', '2025-03-13', 'https://pay.payos.vn/web/7cd96c1dea78424eaa49b9e05d413b9b', '2025-03-11 16:18:22.937'), -- Order 14
(8, 'PENDING', 30000.00, 'PAYOS', 'ONLINE', 'duy', '0399191045', 'russia', '2025-03-11 16:19:08.377', '2025-03-12', '2025-03-13', NULL, '2025-03-11 16:19:08.377'), -- Order 15
(8, 'COMPLETED', 30000.00, 'PAYOS', 'ONLINE', 'duy', '0399191045', 'russia', '2025-03-11 16:21:18.330', '2025-03-12', '2025-03-13', 'https://pay.payos.vn/web/ced7468749454ef08cdcd14cae0a6f26', '2025-03-11 16:22:20.517'), -- Order 16
(8, 'CANCELLED', 45000.00, 'PAYOS', 'ONLINE', 'duy', '0399191045', 'quan 2', '2025-03-11 16:25:46.520', '2025-03-12', '2025-03-13', 'https://pay.payos.vn/web/98e620dc3d334b76bb9658c4debff0b0', '2025-03-11 16:27:37.103'), -- Order 17
(10, 'PENDING', 25000.00, 'PAYOS', 'ONLINE', 'tuấn đạt', '0399191045', 'tp thủ đức', '2025-03-12 06:22:48.200', '2025-03-13', '2025-03-14', NULL, '2025-03-12 06:22:48.200'), -- Order 18
(10, 'CANCELLED', 25000.00, 'PAYOS', 'ONLINE', 'tuấn đạt', '0399191045', 'tp thủ đức', '2025-03-12 06:22:52.427', '2025-03-13', '2025-03-14', 'https://pay.payos.vn/web/d6dbc3ad2d664de9b7263f3eab77a39d', '2025-03-12 06:23:07.223'), -- Order 19
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:24.907', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:24.907'), -- Order 20
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:30.660', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:30.660'), -- Order 21
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:34.980', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:34.980'), -- Order 22
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:36.130', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:36.130'), -- Order 23
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:37.263', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:37.263'), -- Order 24
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:38.043', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:38.043'), -- Order 25
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:38.853', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:38.853'), -- Order 26
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:39.993', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:39.993'), -- Order 27
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:40.707', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:40.707'), -- Order 28
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:41.883', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:41.883'), -- Order 29
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:42.847', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:42.847'), -- Order 30
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:43.400', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:43.400'), -- Order 31
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:44.553', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:44.553'), -- Order 32
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:45.103', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:45.103'), -- Order 33
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:50.400', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:50.400'), -- Order 34
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:51.210', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:51.210'), -- Order 35
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:52.373', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:52.373'), -- Order 36
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:53.173', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:53.173'), -- Order 37
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:54.273', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:54.273'), -- Order 38
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:55.023', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:55.023'), -- Order 39
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:55.930', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:55.930'), -- Order 40
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:58.367', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:58.367'), -- Order 41
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:32:59.160', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:32:59.160'), -- Order 42
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:00.330', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:00.330'), -- Order 43
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:01.413', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:01.413'), -- Order 44
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:02.493', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:02.493'), -- Order 45
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:03.500', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:03.500'), -- Order 46
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:06.730', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:06.730'), -- Order 47
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:07.617', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:07.617'), -- Order 48
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:08.817', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:08.817'), -- Order 49
(10, 'CONFIRMED', 10000.00, 'CASH', 'ONLINE', 'Duy Luong', '0399191045', 'saigon', '2025-03-12 06:33:09.937', '2025-03-12', '2025-03-13', NULL, '2025-03-12 06:33:09.937'), -- Order 50
(11, 'PENDING', 30000.00, 'PAYOS', 'ONLINE', 'dat', '0399191045', 'tp thu duc', '2025-03-14 07:28:39.220', '2025-03-12', '2025-03-13', NULL, '2025-03-12 07:28:39.220'), -- Order 51 (Sửa lại CustomerId từ 14 về 11 cho nhất quán)
(14, 'PENDING', 670000.00, 'PAYOS', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:34:22.213', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:34:22.213'), -- Order 52
(14, 'PENDING', 670000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:34:38.900', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:34:38.900'), -- Order 53
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:13.440', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:13.440'), -- Order 54
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:19.217', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:19.217'), -- Order 55
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:21.497', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:21.497'), -- Order 56
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:23.840', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:23.840'), -- Order 57
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:25.887', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:25.887'), -- Order 58
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:28.263', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:28.263'), -- Order 59
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:30.027', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:30.027'), -- Order 60
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:32.413', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:32.413'), -- Order 61
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:34.813', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:34.813'), -- Order 62
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:36.540', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:36.540'), -- Order 63
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:38.673', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:38.673'), -- Order 64
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:39.717', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:39.717'), -- Order 65
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:42.420', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:42.420'), -- Order 66
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:46.297', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:46.297'), -- Order 67
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:47.850', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:47.850'), -- Order 68
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:49.813', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:49.813'), -- Order 69
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:51.037', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:51.037'), -- Order 70
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:53.267', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:53.267'), -- Order 71
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:55.717', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:55.717'), -- Order 72
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:36:58.110', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:36:58.110'), -- Order 73
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:00.763', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:37:00.763'), -- Order 74
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:06.100', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:37:06.100'), -- Order 75
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:07.327', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:37:07.327'), -- Order 76
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:09.373', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:37:09.373'), -- Order 77
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:11.000', '2025-02-03', '2025-02-05', NULL, '2025-03-12 07:37:11.000'), -- Order 78
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:11.847', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:37:11.847'), -- Order 79 (Sửa lại UpdatedAt cho nhất quán)
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:37:14.907', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:37:14.907'), -- Order 80
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:38:28.567', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:38:28.567'), -- Order 81
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-02-12 07:38:29.410', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:38:29.410'), -- Order 82
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-02-12 07:38:30.190', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:38:30.190'), -- Order 83
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:38:31.563', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:38:31.563'), -- Order 84
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:40:41.553', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:40:41.553'), -- Order 85
(14, 'PENDING', 480000.00, 'CASH', 'ONLINE', 'Tuan Dat Dep Trai', '0925778789', 'string', '2025-03-12 07:40:43.313', '2025-02-03', '2025-02-05', NULL, '2025-02-12 07:40:43.313'), -- Order 86
(11, 'COMPLETED', 45000.00, 'PAYOS', 'ONLINE', 'dat', '0399191045', 'tp thu duc', '2025-03-12 07:43:26.533', '2025-03-12', '2025-03-13', 'https://pay.payos.vn/web/f62dac8f2f3344b1abbaf9a4eb7e0c98', '2025-02-12 07:43:56.983'), -- Order 87 (Sửa lại UpdatedAt cho nhất quán)
(8, 'COMPLETED', 100000.00, 'CASH', 'ONLINE', 'Đoàn Anh Khang', '0374277590', '255 ấp bình An', '2025-02-18 13:23:22.967', '2025-03-19', '2025-03-20', NULL, '2025-02-18 13:23:22.967'), -- Order 88
(8, 'COMPLETED', 180000.00, 'PAYOS', 'ONLINE', 'Lương Ngọc Phương Duy', '0399191045', 'saigon', '2025-03-26 09:20:18.437', '2025-03-26', '2025-03-28', 'https://pay.payos.vn/web/064fa5e62e6b4422b3ae9437211e5033', '2025-03-26 09:20:49.547'), -- Order 89
(27, 'PENDING', 200000.00, 'CASH', 'ONLINE', 'Tuấn Đạt', '0925778789', '334/104/41 Chu Văn An, P.12, Q.Bình Thạnh, TP.HCM', '2025-03-26 18:15:25.190', '2025-03-29', '2025-03-31', NULL, '2025-03-26 18:15:25.190'), -- Order 90
(27, 'COMPLETED', 246000.00, 'PAYOS', 'ONLINE', 'Tuan Dat', '0925778789', '396 Chu Văn An, P.12, Q.Bình Thạnh, TP.HCM', '2025-03-26 18:17:18.757', '2025-03-28', '2025-03-29', 'https://pay.payos.vn/web/a51e6846587242ecbb443c6048be7411', '2025-03-26 18:17:56.833'); -- Order 91




-- Dữ liệu mẫu tự thêm cho các Id từ 1 đến 9 (Ghi chú: Đây là dữ liệu do tôi tự tạo)
-- Mục đích: Điền các bản ghi thiếu để đảm bảo thứ tự liên tục
INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price, TotalPrice, Status) VALUES
(1, 1, 1, 50000.00, 50000.00, 'ACTIVE'), -- OrderItem 1
(2, 2, 1, 75000.00, 75000.00, 'PENDING'), -- OrderItem 2
(3, 3, 2, 40000.00, 80000.00, 'COMPLETED'), -- OrderItem 3
(4, 4, 1, 30000.00, 30000.00, 'CANCELLED'), -- OrderItem 4
(5, 5, 1, 20000.00, 20000.00, 'ACTIVE'), -- OrderItem 5
(6, 6, 2, 25000.00, 50000.00, 'COMPLETED'), -- OrderItem 6
(7, 7, 1, 60000.00, 60000.00, 'PENDING'), -- OrderItem 7
(8, 8, 1, 25000.00, 25000.00, 'CANCELLED'), -- OrderItem 8
(9, 9, 2, 40000.00, 80000.00, 'ACTIVE'); -- OrderItem 9

-- Dữ liệu thực tế bạn cung cấp từ Id 10 đến 62
INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price, TotalPrice, Status) VALUES
(11, 1, 1, 15000.00, 15000.00, 'ACTIVE'), -- OrderItem 10
(11, 10, 1, 25000.00, 25000.00, 'ACTIVE'), -- OrderItem 11
(12, 11, 1, 15000.00, 15000.00, 'ACTIVE'), -- OrderItem 12
(13, 11, 1, 15000.00, 15000.00, 'ACTIVE'), -- OrderItem 13
(14, 11, 1, 15000.00, 15000.00, 'CANCELLED'), -- OrderItem 14
(15, 11, 2, 15000.00, 30000.00, 'ACTIVE'), -- OrderItem 15
(16, 11, 2, 15000.00, 30000.00, 'ACTIVE'), -- OrderItem 16
(17, 11, 3, 15000.00, 45000.00, 'CANCELLED'), -- OrderItem 17
(18, 10, 1, 25000.00, 25000.00, 'ACTIVE'), -- OrderItem 18
(19, 10, 1, 25000.00, 25000.00, 'CANCELLED'), -- OrderItem 19
(51, 11, 2, 15000.00, 30000.00, 'ACTIVE'), -- OrderItem 20 (Sửa lại Id thành 20 cho thứ tự)
(87, 11, 3, 15000.00, 45000.00, 'ACTIVE'), -- OrderItem 58 (Sửa lại Id thành 21)
(88, 16, 1, 100000.00, 100000.00, 'ACTIVE'), -- OrderItem 59 (Sửa lại Id thành 22)
(89, 15, 1, 180000.00, 180000.00, 'ACTIVE'), -- OrderItem 60 (Sửa lại Id thành 23)
(90, 16, 1, 100000.00, 100000.00, 'ACTIVE'), -- OrderItem 61 (Sửa lại Id thành 24)
(91, 14, 2, 123000.00, 246000.00, 'ACTIVE'); -- OrderItem 62 (Sửa lại Id thành 25)