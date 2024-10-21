DROP DATABASE IF EXISTS CourseWorkProto1;

CREATE DATABASE IF NOT EXISTS CourseWorkProto1;

USE CourseWorkProto1;

-- Таблиця для користувачів (знайомих)
CREATE TABLE Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Login NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Gender INT NOT NULL,
    Role INT NOT NULL
);

-- Таблиця для продуктів
CREATE TABLE Products (
    ProductId INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(255) NOT NULL,
    ProductType INT NOT NULL,
    OwnerId INT NOT NULL,
    FOREIGN KEY (OwnerId) REFERENCES Users(UserId) ON DELETE CASCADE
);

-- Таблиця для дисків
CREATE TABLE Discs (
    DiscId INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    Format INT NOT NULL,
    Year INT,
    DiscType INT NOT NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);

-- Таблиця для фільмів
CREATE TABLE Movies (
    MovieId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    Duration INT NOT NULL,
    Director NVARCHAR(255),
    Genre INT NOT NULL,
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE
);

-- Таблиця для акторів
CREATE TABLE Actors (
    ActorId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(255) NOT NULL
);

-- Таблиця для зв'язку фільмів з акторами
CREATE TABLE ActorMovie (
    MovieId INT NOT NULL,
    ActorId INT NOT NULL,
    FOREIGN KEY (MovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (ActorId) REFERENCES Actors(ActorId) ON DELETE CASCADE ON UPDATE CASCADE,
    PRIMARY KEY (MovieId, ActorId)
);

-- Таблиця для музичних дисків
CREATE TABLE Music (
    MusicId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    Artist NVARCHAR(255),
    Genre INT NOT NULL,
    TrackCount INT,
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE
);

-- Таблиця для ігор
CREATE TABLE Games (
    GameId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    Platform INT NOT NULL,
    Genre INT NOT NULL,
    Developer NVARCHAR(255),
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE
);

-- Таблиця для книг
CREATE TABLE Books (
    BookId INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    Author NVARCHAR(255),
    Genre INT NOT NULL,
    PublicationYear INT,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);

-- Таблиця для позичень
CREATE TABLE Borrows (
    BorrowId INT PRIMARY KEY AUTO_INCREMENT,
    LenderId INT NOT NULL, -- Хто позичає
    BorrowerId INT NOT NULL, -- Хто бере у позичку
    ProductId INT NOT NULL,
    BorrowStartDate DATE NOT NULL,
    BorrowEndDate DATE,
    FOREIGN KEY (LenderId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (BorrowerId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);

-- Таблиця для оренди
CREATE TABLE Rentals (
    RentalId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    RentalStartDate DATE NOT NULL,
    RentalEndDate DATE,
    PaymentAmount DECIMAL(10, 2),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE
);
