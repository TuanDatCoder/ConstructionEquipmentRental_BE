
CREATE DATABASE ConstructionEquipmentRentalDB;
GO

USE ConstructionEquipmentRentalDB;
GO

-- Tạo bảng Store
CREATE TABLE Store (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Address NVARCHAR(MAX),
    Phone NVARCHAR(20),
    Status NVARCHAR(50) DEFAULT 'ACTIVE',
    OpeningDay DATE,
    OpeningHours TIME,
    ClosingHours TIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE(),
    BusinessLicense NVARCHAR(MAX)
);

-- Tạo bảng Account
CREATE TABLE Account (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StoreId INT NULL FOREIGN KEY REFERENCES Store(Id),
    Username NVARCHAR(255),
    Password NVARCHAR(MAX),
    Email NVARCHAR(255),
    Phone NVARCHAR(20),
    Address NVARCHAR(MAX),
    DateOfBirth DATE,
    Picture NVARCHAR(MAX),
    GoogleId NVARCHAR(255),
    Role NVARCHAR(50) DEFAULT 'CUSTOMER',
    Status NVARCHAR(50) DEFAULT 'ACTIVE',
    Points INT DEFAULT 0,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Tạo bảng Brand
CREATE TABLE Brand (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255),
    Description NVARCHAR(MAX),
    Country NVARCHAR(255),
    Status NVARCHAR(50) DEFAULT 'ACTIVE'
);

-- Tạo bảng Category
CREATE TABLE Category (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(255),
    Description NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'ACTIVE'
);

-- Tạo bảng Product
CREATE TABLE Product (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CategoryId INT NOT NULL FOREIGN KEY REFERENCES Category(Id),
    BrandId INT NOT NULL FOREIGN KEY REFERENCES Brand(Id),
    StoreId INT NOT NULL FOREIGN KEY REFERENCES Store(Id),
    Name NVARCHAR(255),
    Description NVARCHAR(MAX),
    DefaultImage NVARCHAR(MAX),
    Price DECIMAL(18,2),
    Discount DECIMAL(18,2),
    PriceSale DECIMAL(18,2),
    Stock INT,
    Status NVARCHAR(50) DEFAULT 'AVAILABLE',
    DiscountStartDate DATE,
    DiscountEndDate DATE,
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Tạo bảng ProductImage
CREATE TABLE ProductImage (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    ImageUrl NVARCHAR(MAX)
);

-- Tạo bảng Order
CREATE TABLE [Order] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    StaffId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    Status NVARCHAR(50) DEFAULT 'PENDING',
    TotalPrice DECIMAL(18,2),
    PaymentMethod NVARCHAR(50) DEFAULT 'CASH',
    PurchaseMethod NVARCHAR(50) DEFAULT 'ONLINE',
    RecipientName NVARCHAR(255),
    RecipientPhone NVARCHAR(20),
    Address NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    DateOfReceipt DATE,
    DateOfReturn DATE
);

-- Tạo bảng OrderItem
CREATE TABLE OrderItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    Quantity INT,
    Price DECIMAL(18,2)
);
-- Tạo bảng Feedback
CREATE TABLE Feedback (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AccountId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    ProductId INT NOT NULL FOREIGN KEY REFERENCES Product(Id),
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    Rating INT CHECK (Rating BETWEEN 1 AND 5),
    Comment NVARCHAR(MAX),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE()
);

-- Tạo bảng OrderReport
CREATE TABLE OrderReport (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    ReporterId INT NOT NULL FOREIGN KEY REFERENCES Account(Id),
    HandlerId INT NULL FOREIGN KEY REFERENCES Account(Id),
    Reason NVARCHAR(MAX),
    Details NVARCHAR(MAX),
    Status NVARCHAR(50) DEFAULT 'PENDING',
    CreatedAt DATETIME DEFAULT GETDATE(),
    ResolvedAt DATETIME
);

-- Tạo bảng Transaction
CREATE TABLE [Transaction] (
    Id UNIQUEIDENTIFIER DEFAULT NEWID() PRIMARY KEY,
    OrderId INT NOT NULL FOREIGN KEY REFERENCES [Order](Id),
    TransactionCode NVARCHAR(255),
    Amount DECIMAL(18,2),
    Status NVARCHAR(50),
    PaymentMethod NVARCHAR(50),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE RefreshToken (
   RefreshTokenID INT PRIMARY KEY IDENTITY(1,1),
   ExpiredAt Datetime NOT NULL,
   Token VARCHAR (255) NOT NULL,
   AccountID INT,
   FOREIGN KEY (AccountID) REFERENCES Account(Id),
  
);


-- -----------------------------------------Them Du Lieu-------------------------------------------------
INSERT INTO Store (Name, Address, Phone, Status, OpeningDay, OpeningHours, ClosingHours, BusinessLicense)
VALUES
('Store A', '123 Main St', '0123456789', 'ACTIVE', '2022-01-01', '08:00:00', '22:00:00', 'BL001'),
('Store B', '456 Elm St', '0987654321', 'ACTIVE', '2022-02-01', '09:00:00', '21:00:00', 'BL002'),
('Store C', '789 Oak St', '0234567890', 'INACTIVE', '2022-03-01', '10:00:00', '20:00:00', 'BL003'),
('Store D', '321 Pine St', '0345678901', 'ACTIVE', '2022-04-01', '07:00:00', '23:00:00', 'BL004'),
('Store E', '654 Maple St', '0456789012', 'ACTIVE', '2022-05-01', '08:30:00', '22:30:00', 'BL005');
INSERT INTO Account (StoreId, Username, Password, Email, Phone, Address, DateOfBirth, Picture, Role, Status, Points)
VALUES
(1, 'user1', 'password1', 'user1@example.com', '0123456789', '123 Main St', '1990-01-01', NULL, 'CUSTOMER', 'ACTIVE', 100),
(2, 'user2', 'password2', 'user2@example.com', '0987654321', '456 Elm St', '1992-02-02', NULL, 'CUSTOMER', 'ACTIVE', 200),
(3, 'staff1', 'password3', 'staff1@example.com', '0234567890', '789 Oak St', '1988-03-03', NULL, 'STAFF', 'ACTIVE', 0),
(4, 'staff2', 'password4', 'staff2@example.com', '0345678901', '321 Pine St', '1985-04-04', NULL, 'STAFF', 'ACTIVE', 0),
(5, 'admin', 'adminpass', 'admin@example.com', '0456789012', '654 Maple St', '1980-05-05', NULL, 'ADMIN', 'ACTIVE', 0);
INSERT INTO Brand (Name, Description, Country, Status)
VALUES
('Brand A', 'High-quality brand from USA', 'USA', 'ACTIVE'),
('Brand B', 'Affordable brand from Vietnam', 'Vietnam', 'ACTIVE'),
('Brand C', 'Luxury brand from France', 'France', 'ACTIVE'),
('Brand D', 'Eco-friendly brand from Germany', 'Germany', 'ACTIVE'),
('Brand E', 'Innovative brand from Japan', 'Japan', 'ACTIVE');
INSERT INTO Category (Name, Description, Status)
VALUES
('Electronics', 'Devices and gadgets', 'ACTIVE'),
('Fashion', 'Clothing and accessories', 'ACTIVE'),
('Books', 'Books and literature', 'ACTIVE'),
('Home Appliances', 'Tools and appliances for home', 'ACTIVE'),
('Sports', 'Sports gear and equipment', 'ACTIVE');
INSERT INTO Product (CategoryId, BrandId, StoreId, Name, Description, DefaultImage, Price, Discount, PriceSale, Stock, Status)
VALUES
(1, 1, 1, 'Smartphone A', 'Latest smartphone from Brand A', '/images/smartphoneA.jpg', 500.00, 50.00, 450.00, 100, 'AVAILABLE'),
(2, 2, 2, 'T-shirt B', 'Comfortable T-shirt from Brand B', '/images/tshirtB.jpg', 20.00, 2.00, 18.00, 200, 'AVAILABLE'),
(3, 3, 3, 'Book C', 'Bestseller book from Brand C', '/images/bookC.jpg', 15.00, 1.50, 13.50, 50, 'AVAILABLE'),
(4, 4, 4, 'Vacuum Cleaner D', 'Powerful vacuum cleaner from Brand D', '/images/vacuumD.jpg', 300.00, 30.00, 270.00, 30, 'AVAILABLE'),
(5, 5, 5, 'Running Shoes E', 'Durable running shoes from Brand E', '/images/shoesE.jpg', 100.00, 10.00, 90.00, 150, 'AVAILABLE');
INSERT INTO [Order] (CustomerId, StaffId, Status, TotalPrice, PaymentMethod, PurchaseMethod, RecipientName, RecipientPhone, Address, DateOfReceipt)
VALUES
(1, 3, 'PENDING', 450.00, 'CASH', 'ONLINE', 'John Doe', '0123456789', '123 Main St', '2024-01-01'),
(2, 4, 'PENDING', 18.00, 'CASH', 'ONLINE', 'Jane Smith', '0987654321', '456 Elm St', '2024-02-01'),
(3, 3, 'COMPLETED', 13.50, 'CASH', 'OFFLINE', 'Alice Brown', '0234567890', '789 Oak St', '2024-03-01'),
(4, 4, 'COMPLETED', 270.00, 'CARD', 'ONLINE', 'Bob Green', '0345678901', '321 Pine St', '2024-04-01'),
(5, 5, 'CANCELLED', 90.00, 'CARD', 'ONLINE', 'Charlie White', '0456789012', '654 Maple St', '2024-05-01');

INSERT INTO ProductImage (ProductId, ImageUrl)
VALUES 
    (1, 'https://example.com/images/product1_1.jpg'),
    (1, 'https://example.com/images/product1_2.jpg'),
    (2, 'https://example.com/images/product2_1.jpg'),
    (3, 'https://example.com/images/product3_1.jpg'),
    (3, 'https://example.com/images/product3_2.jpg');
INSERT INTO OrderItem (OrderId, ProductId, Quantity, Price)
VALUES 
    (1, 1, 2, 200.00),
    (1, 2, 1, 150.00),
    (2, 3, 3, 300.00),
    (3, 4, 1, 100.00),
    (3, 5, 2, 250.00);
INSERT INTO Feedback (AccountId, ProductId, OrderId, Rating, Comment)
VALUES 
    (1, 1, 1, 5, 'Great product! Highly recommended.'),
    (2, 2, 1, 4, 'Good quality, but delivery was slow.'),
    (3, 3, 2, 3, 'Average experience.'),
    (4, 4, 3, 5, 'Perfect! Will buy again.'),
    (5, 5, 3, 2, 'Not satisfied with the product.');
INSERT INTO OrderReport (OrderId, ReporterId, HandlerId, Reason, Details, Status, ResolvedAt)
VALUES 
    (1, 2, 3, 'Damaged product', 'The product was damaged upon arrival.', 'RESOLVED', '2024-12-01 10:30:00'),
    (2, 3, NULL, 'Incorrect item', 'Received the wrong product.', 'PENDING', NULL),
    (3, 4, 1, 'Late delivery', 'The delivery was delayed by 3 days.', 'RESOLVED', '2024-12-15 14:00:00'),
    (4, 5, NULL, 'Missing items', 'Some items were missing in the order.', 'PENDING', NULL),
    (5, 1, 2, 'Refund request', 'Requesting a refund for defective product.', 'RESOLVED', '2024-12-20 09:00:00');
INSERT INTO [Transaction] (OrderId, TransactionCode, Amount, Status, PaymentMethod)
VALUES 
    (1, 'TXN001', 350.00, 'SUCCESS', 'CASH'),
    (2, 'TXN002', 300.00, 'SUCCESS', 'BANKING'),
    (3, 'TXN003', 450.00, 'FAILED', 'VNPAY'),
    (4, 'TXN004', 200.00, 'PENDING', 'BANKING'),
    (5, 'TXN005', 150.00, 'SUCCESS', 'CASH');
INSERT INTO RefreshToken (ExpiredAt, Token, AccountID)
VALUES 
    ('2024-12-31 23:59:59', 'token123abc', 1),
    ('2024-12-31 23:59:59', 'token456def', 2),
    ('2024-12-31 23:59:59', 'token789ghi', 3),
    ('2024-12-31 23:59:59', 'token101jkl', 4),
    ('2024-12-31 23:59:59', 'token112mno', 5);
