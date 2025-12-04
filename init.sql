CREATE DATABASE AvitoMiniDB;
GO

USE AvitoMiniDB;
GO

CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    Login NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE()
);

CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Ads (
    AdId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    CategoryId INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    Price DECIMAL(18,2) NOT NULL,
    City NVARCHAR(100),
    ImagePath NVARCHAR(500),
    Status NVARCHAR(20) DEFAULT 'Active',
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

CREATE TABLE CompletedAds (
    CompletedAdId INT PRIMARY KEY IDENTITY(1,1),
    AdId INT NOT NULL,
    FinalPrice DECIMAL(18,2) NOT NULL,
    CompletedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (AdId) REFERENCES Ads(AdId) ON DELETE CASCADE
);

INSERT INTO Categories (Name) VALUES 
    (N'Товары'),
    (N'Услуги'),
    (N'Недвижимость'),
    (N'Авто');

INSERT INTO Users (Login, Password, Email) VALUES 
    ('admin', 'admin123', 'admin@avito.local'),
    ('user1', 'pass123', 'user1@avito.local');

INSERT INTO Ads (UserId, CategoryId, Title, Description, Price, City, Status) VALUES
    (1, 1, N'iPhone 15 Pro', N'Новый телефон в отличном состоянии', 85000, N'Москва', 'Active'),
    (1, 3, N'Квартира 2-комнатная', N'Продам квартиру в центре', 8500000, N'Санкт-Петербург', 'Active'),
    (2, 4, N'BMW X5 2020', N'Продам автомобиль в хорошем состоянии', 4500000, N'Казань', 'Active');

GO
