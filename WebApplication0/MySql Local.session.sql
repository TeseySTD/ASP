-- SET FOREIGN_KEY_CHECKS = 0;

-- DROP TABLE courses;

-- DROP TABLE students;

-- DROP TABLE enrollments;

-- DROP TABLE users

SET FOREIGN_KEY_CHECKS = 1;

-- CREATE TABLE students (
--     ID INT AUTO_INCREMENT PRIMARY KEY,
--     LastName NVARCHAR(50) NOT NULL,
--     FirstMidName NVARCHAR(50) NOT NULL,
--     EnrollmentDate DATETIME NOT NULL
-- );

-- CREATE TABLE courses (
--     CourseID INT PRIMARY KEY,
--     Title NVARCHAR(50) NOT NULL,
--     Credits INT NOT NULL
-- );

-- CREATE TABLE enrollments (
--     EnrollmentID INT AUTO_INCREMENT PRIMARY KEY,
--     CourseID INT NOT NULL,
--     StudentID INT NOT NULL,
--     Grade INT NULL,
--     FOREIGN KEY (CourseID) REFERENCES courses(CourseID) ON UPDATE CASCADE ON DELETE CASCADE,
--     FOREIGN KEY (StudentID) REFERENCES students(ID) ON UPDATE CASCADE ON DELETE CASCADE
-- );