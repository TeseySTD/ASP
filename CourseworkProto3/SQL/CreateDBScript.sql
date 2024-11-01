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
    OwnerId INT,
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
CREATE TABLE Music (
    MusicId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    TrackCount INT,
    Artist NVARCHAR(255),
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE
);

-- Таблиця для ігор
CREATE TABLE Games (
    GameId INT PRIMARY KEY AUTO_INCREMENT,
    DiscId INT NOT NULL,
    Developer NVARCHAR(255),
    Publisher NVARCHAR(255),
    FOREIGN KEY (DiscId) REFERENCES Discs(DiscId) ON DELETE CASCADE
);

-- Таблиця для книг
CREATE TABLE Books (
    BookId INT PRIMARY KEY AUTO_INCREMENT,
    ProductId INT NOT NULL,
    Author NVARCHAR(255),
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

-- Таблиця для замовленнь продуктів 
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT, -- користувач, який  замовляє
    ProductId INT, -- продукт, який замовлений
    StartDate DATE, -- дата початку оренди 
    EndDate DATE, -- дата завершення оренди
    PaymentAmount DECIMAL(7, 2) DEFAULT 0.0,
    Status INT, -- статус замовлення
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE SET NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE SET NULL
);


-- Таблиця для жанрів книг
CREATE TABLE BookGenres (
    GenreId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(255) NOT NULL
);

-- Таблиця для жанрів музики
CREATE TABLE MusicGenres (
    GenreId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(255) NOT NULL
);

-- Таблиця для жанрів фільмів
CREATE TABLE MovieGenres (
    GenreId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(255) NOT NULL
);

-- Таблиця для жанрів ігор
CREATE TABLE GameGenres (
    GenreId INT PRIMARY KEY AUTO_INCREMENT,
    Name NVARCHAR(255) NOT NULL
);

-- Таблиця для зв'язку книг і жанрів
CREATE TABLE BookBookGenre (
    BooksBookId INT NOT NULL,
    GenreId INT NOT NULL,
    FOREIGN KEY (BooksBookId) REFERENCES Books(BookId) ON DELETE CASCADE,
    FOREIGN KEY (GenreId) REFERENCES BookGenres(GenreId) ON DELETE CASCADE,
    PRIMARY KEY (BooksBookId, GenreId)
);

-- Таблиця для зв'язку музики і жанрів
CREATE TABLE MusicMusicGenre (
    MusicId INT NOT NULL,
    GenreId INT NOT NULL,
    FOREIGN KEY (MusicId) REFERENCES Music(MusicId) ON DELETE CASCADE,
    FOREIGN KEY (GenreId) REFERENCES MusicGenres(GenreId) ON DELETE CASCADE,
    PRIMARY KEY (MusicId, GenreId)
);

-- Таблиця для зв'язку фільмів і жанрів
CREATE TABLE MovieMovieGenre (
    MoviesMovieId INT NOT NULL,
    GenreId INT NOT NULL,
    FOREIGN KEY (MoviesMovieId) REFERENCES Movies(MovieId) ON DELETE CASCADE,
    FOREIGN KEY (GenreId) REFERENCES MovieGenres(GenreId) ON DELETE CASCADE,
    PRIMARY KEY (MoviesMovieId, GenreId)
);

-- Таблиця для зв'язку ігор і жанрів
CREATE TABLE GameGameGenre (
    GamesGameId INT NOT NULL,
    GenreId INT NOT NULL,
    FOREIGN KEY (GamesGameId) REFERENCES Games(GameId) ON DELETE CASCADE,
    FOREIGN KEY (GenreId) REFERENCES GameGenres(GenreId) ON DELETE CASCADE,
    PRIMARY KEY (GamesGameId, GenreId)
);
