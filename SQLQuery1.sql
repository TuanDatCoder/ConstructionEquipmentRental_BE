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