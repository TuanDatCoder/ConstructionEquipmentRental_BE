
CREATE DATABASE ConstructionEquipmentRentalDB;
GO

USE ConstructionEquipmentRentalDB;
GO

-- Tạo bảng Account
CREATE TABLE Account (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StoreId INT NULL, -- Cho phép NULL
    Username NVARCHAR(255) NOT NULL,
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


-- -----------------------------------------Them Du Lieu-------------------------------------------------
-- Thêm dữ liệu mẫu vào bảng Account
INSERT INTO Account (StoreId, Username, Password, Email, Phone, Address, DateOfBirth, Role, Status, Points) VALUES
(1, 'storeowner1', 'hashed_password1', 'owner1@store.com', '1234567890', '123 Main St', '1980-01-01', 'STORE_OWNER', 'ACTIVE', 500),
(2, 'storeowner2', 'hashed_password2', 'owner2@store.com', '0987654321', '456 Market Ave', '1985-02-15', 'STORE_OWNER', 'ACTIVE', 300),
(3, 'customer1', 'hashed_password3', 'customer1@gmail.com', '1122334455', '789 Elm St', '1995-06-10', 'CUSTOMER', 'ACTIVE', 100),
(4, 'customer2', 'hashed_password4', 'customer2@gmail.com', '6677889900', '321 Oak St', '1992-03-22', 'CUSTOMER', 'ACTIVE', 200),
(5, 'admin1', 'hashed_password5', 'admin@system.com', '5544332211', 'Admin Office', '1975-12-05', 'ADMIN', 'ACTIVE', 0);

-- Thêm dữ liệu mẫu vào bảng Store
INSERT INTO Store (Name, Address, Phone, OpeningDay, OpeningHours, ClosingHours, BusinessLicense, AccountId) VALUES
('Store 1', '123 Main St', '1234567890', '2010-05-20', '08:00', '18:00', 'BL12345', 1),
('Store 2', '456 Market Ave', '0987654321', '2012-07-15', '09:00', '19:00', 'BL67890', 2),
('Store 3', '789 Central Blvd', '2233445566', '2015-03-10', '07:30', '17:30', 'BL11223', 3),
('Store 4', '321 Elm St', '4455667788', '2018-11-01', '08:00', '18:30', 'BL44567', 4),
('Store 5', '987 Park Ln', '6677889900', '2020-09-25', '09:30', '20:00', 'BL99876', 5);

-- Thêm dữ liệu mẫu vào bảng Brand
INSERT INTO Brand (Name, Description, Country) VALUES
('Caterpillar', 'Leading manufacturer of construction equipment.', 'USA'),
('Komatsu', 'World-renowned producer of construction machinery.', 'Japan'),
('Hitachi', 'Specializes in heavy equipment for construction.', 'Japan'),
('Volvo', 'Offers innovative construction machinery solutions.', 'Sweden'),
('JCB', 'Pioneer in construction equipment manufacturing.', 'UK');

-- Thêm dữ liệu mẫu vào bảng Category
INSERT INTO Category (Name, Description) VALUES
('Excavators', 'Heavy machinery for digging and moving earth.'),
('Loaders', 'Equipment for lifting and transporting materials.'),
('Cranes', 'Machinery for lifting heavy loads.'),
('Bulldozers', 'Machinery for pushing large amounts of material.'),
('Concrete Mixers', 'Equipment for mixing concrete on site.');

-- Thêm dữ liệu mẫu vào bảng Product
INSERT INTO Product (CategoryId, BrandId, StoreId, Name, Description, DefaultImage, Price, Stock, Weight, Dimensions, FuelType) VALUES
(1, 1, 1, 'CAT 320 Excavator', 'High-performance excavator for tough jobs.', 'cat320.jpg', 120000.00, 5, 22.5, '10x3x3 m', 'DIESEL'),
(2, 2, 1, 'Komatsu WA380 Loader', 'Versatile loader for various construction tasks.', 'wa380.jpg', 95000.00, 3, 18.0, '9x3x3 m', 'DIESEL'),
(3, 3, 2, 'Hitachi ZX200 Excavator', 'Efficient excavator with advanced hydraulics.', 'zx200.jpg', 115000.00, 4, 21.0, '10x3x3 m', 'DIESEL'),
(4, 4, 2, 'Volvo L110 Loader', 'Reliable loader with excellent fuel efficiency.', 'l110.jpg', 105000.00, 2, 20.0, '9x3x3 m', 'DIESEL'),
(5, 5, 3, 'JCB 3CX Backhoe Loader', 'Multi-purpose machine for construction sites.', '3cx.jpg', 75000.00, 6, 8.5, '6x2.5x2.5 m', 'DIESEL');

-- Thêm dữ liệu mẫu vào bảng ProductImage
INSERT INTO ProductImage (ProductId, ImageUrl) VALUES
(1, 'images/cat320_front.jpg'),
(1, 'images/cat320_side.jpg'),
(2, 'images/wa380_front.jpg'),
(3, 'images/zx200_front.jpg'),
(4, 'images/l110_front.jpg');

-- Thêm dữ liệu mẫu vào bảng Order
INSERT INTO [Order] (CustomerId, TotalPrice, RecipientName, RecipientPhone, Address, DateOfReceipt, DateOfReturn) VALUES
(3, 1200.00, 'John Doe', '1234567890', '123 Main St', '2024-01-01', '2024-01-05'),
(4, 950.00, 'Jane Smith', '0987654321', '456 Market Ave', '2024-01-02', '2024-01-06'),
(3, 1150.00, 'Alice Brown', '2233445566', '789 Elm St', '2024-01-03', '2024-01-07'),
(4, 1050.00, 'Bob White', '4455667788', '321 Oak St', '2024-01-04', '2024-01-08'),
(3, 750.00, 'Charlie Black', '6677889900', '987 Park Ln', '2024-01-05', '2024-01-09');

-- Thêm dữ liệu mẫu vào bảng OrderItem
INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price, TotalPrice) VALUES
(1, 1, 1, 120000.00, 1200.00),
(2, 2, 1, 95000.00, 950.00),
(3, 3, 1, 115000.00, 1150.00),
(4, 4, 1, 105000.00, 1050.00),
(5, 5, 1, 75000.00, 750.00);

-- Thêm dữ liệu mẫu vào bảng Feedback
INSERT INTO Feedback (AccountId, ProductId, OrderId, Rating, Comment) VALUES
(3, 1, 1, 5, 'Great product, very efficient!'),
(4, 2, 2, 4, 'Good performance, but a bit noisy.'),
(3, 3, 3, 5, 'Highly recommend this equipment!'),
(4, 4, 4, 3, 'Average experience, expected better.'),
(3, 5, 5, 4, 'Good value for the price.');

-- Thêm dữ liệu mẫu vào bảng OrderReport
INSERT INTO OrderReport (OrderId, ReporterId, Reason, Details) VALUES
(1, 3, 'Late Delivery', 'The product was delivered one day late.'),
(2, 4, 'Damaged Item', 'The equipment was slightly damaged upon delivery.'),
(3, 3, 'Incorrect Item', 'Received the wrong product.'),
(4, 4, 'Poor Customer Service', 'Customer support was unhelpful.'),
(5, 3, 'Overcharged', 'Charged more than the listed price.');

-- Thêm dữ liệu mẫu vào bảng Transaction
INSERT INTO [Transaction] (OrderId, AccountId, PaymentMethod, TotalPrice) VALUES
(1, 3, 'TRANSFER', 1200.00),
(2, 4, 'TRANSFER', 950.00),
(3, 3, 'TRANSFER', 1150.00),
(4, 4, 'TRANSFER', 1050.00),
(5, 3, 'TRANSFER', 750.00);

-- Thêm dữ liệu mẫu vào bảng Wallet
INSERT INTO Wallet (AccountId, BankName, BankAccount, Balance) VALUES
(3, 'Bank A', '12345678', 5000.00),
(4, 'Bank B', '87654321', 3000.00),
(1, 'Bank C', '11112222', 7000.00),
(2, 'Bank D', '33334444', 4000.00),
(5, 'Bank E', '55556666', 6000.00);

-- Thêm dữ liệu mẫu vào bảng WalletLog
INSERT INTO WalletLog (WalletId, TransactionId, Type, Amount) VALUES
(1, 1, 'SUBTRACT', 1200.00),
(2, 2, 'SUBTRACT', 950.00),
(3, 3, 'SUBTRACT', 1150.00),
(4, 4, 'SUBTRACT', 1050.00),
(5, 5, 'SUBTRACT', 750.00);


-- Thêm dữ liệu mẫu vào bảng Cart
INSERT INTO Cart (AccountId, CreatedAt, UpdatedAt, Status) VALUES
(3, GETDATE(), GETDATE(), 'ACTIVE'),  
(4, GETDATE(), GETDATE(), 'PENDING'), 
(3, GETDATE(), GETDATE(), 'COMPLETED'),  
(4, GETDATE(), GETDATE(), 'CANCELLED'); 


-- Thêm dữ liệu mẫu vào bảng CartItem
INSERT INTO CartItem (CartId, ProductId, Quantity, Price) VALUES
(1, 101, 2, 1500.00), 
(1, 102, 1, 3000.00),  
(2, 103, 3, 1000.00), 
(2, 104, 1, 2500.00);  