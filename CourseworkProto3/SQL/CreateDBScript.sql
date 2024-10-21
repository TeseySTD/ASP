DROP DATABASE IF EXISTS CourseWorkProto1;

CREATE DATABASE IF NOT EXISTS CourseWorkProto1;

USE CourseWorkProto1;



-- Таблиця для продуктів
CREATE TABLE Products (
    ProductId INT PRIMARY KEY AUTO_INCREMENT,
    Title NVARCHAR(255) NOT NULL,
    ProductType INT NOT NULL,
    BorrowedDate DATE,
    DueDate DATE,
    IsBorrowed BIT NOT NULL
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
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE
);

-- Таблиця для акторів
CREATE TABLE Actors (
    ActorId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(255) NOT NULL
);

-- Таблиця для зв'язку фільмів з акторами
CREATE TABLE ActorMovie (
    MoviesMovieId INT NOT NULL,
    ActorsActorId INT NOT NULL,
    FOREIGN KEY (MoviesMovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (ActorsActorId) REFERENCES Actors(ActorId) ON DELETE CASCADE ON UPDATE CASCADE,
    PRIMARY KEY (MoviesMovieId, ActorsActorId)
);

-- Таблиця для музичних дисків
CREATE TABLE MusicDiscs (
    MusicDiscId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    Artist NVARCHAR(255),
    MusicGenre INT NOT NULL,
    TrackCount INT,
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Таблиця для ігор
CREATE TABLE Games (
    GameId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    Platform INT NOT NULL,
    Developer NVARCHAR(255),
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Таблиця для книг
CREATE TABLE Books (
    BookId INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    Author NVARCHAR(255),
    Genre NVARCHAR(255),
    PublicationYear INT,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Таблиця для користувачів (знайомих)
CREATE TABLE Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Login NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255),
    Gender INT NOT NULL,
    AccessRights INT NOT NULL
);

-- Таблиця для позичень
CREATE TABLE Borrows (
    BorrowId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT NOT NULL,
    ProductId INT NOT NULL,
    BorrowedDate DATE NOT NULL,
    DueDate DATE,
    IsReturned BIT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE CASCADE ON UPDATE CASCADE
);