CREATE DATABASE Medical_center;
GO

USE Medical_center;
GO

CREATE TABLE Departments (
    DepartmentID INT PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL
);

CREATE TABLE DoctorCategories (
    CategoryID INT PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL
);

CREATE TABLE Doctors (
    DoctorID INT PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    DepartmentID INT,
    CategoryID INT,
    Photo VARBINARY(MAX),
    FOREIGN KEY (DepartmentID) REFERENCES Departments(DepartmentID),
    FOREIGN KEY (CategoryID) REFERENCES DoctorCategories(CategoryID)
);

CREATE TABLE Specializations (
    SpecializationID INT PRIMARY KEY,
    SpecializationName NVARCHAR(100) NOT NULL
);

DROP TABLE Appointments;

CREATE TABLE Appointments (
    AppointmentID INT PRIMARY KEY IDENTITY,
    PatientName NVARCHAR(100) NOT NULL,
    DoctorID INT,
    AppointmentDateTime DATETIME,
    IsCancelled BIT DEFAULT 0,
    FOREIGN KEY (DoctorID) REFERENCES Doctors(DoctorID)
);

INSERT INTO Departments (DepartmentID, DepartmentName)
VALUES 
    (1, 'Cardiology'),
    (2, 'Psychiatry'),
    (3, 'Neurology'),
    (4, 'Pediatrics');

INSERT INTO DoctorCategories (CategoryID, CategoryName)
VALUES 
    (1, 'Cardiologist'),
    (2, 'Psychiatry'),
    (3, 'Neurologist'),
    (4, 'Pediatrician');

INSERT INTO Doctors (DoctorID, FullName, DepartmentID, CategoryID, Photo)
VALUES 
    (1, 'Maxim Kyxapka', 1, 1, (SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Users\examp\Downloads\photo_2024-04-27_17-36-35.jpg', SINGLE_BLOB) AS ImageData)),
    (2, 'Ivan Kabachkov', 2, 2, (SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Users\examp\Downloads\photo_2024-03-30_09-46-27.jpg', SINGLE_BLOB) AS ImageData)),
    (3, 'Pavel Synkaruk', 3, 3, (SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Users\examp\Downloads\photo_2024-02-14_01-29-13.jpg', SINGLE_BLOB) AS ImageData)),
    (4, 'Pavel Labkovski', 4, 4, (SELECT BulkColumn FROM OPENROWSET(BULK 'C:\Users\examp\Downloads\labkovski.png', SINGLE_BLOB) AS ImageData));

INSERT INTO Specializations (SpecializationID, SpecializationName)
VALUES 
    (1, 'Heart Disease'),
    (2, 'Schizophrenia'),
    (3, 'Brain Disorders'),
    (4, 'Child Health');


INSERT INTO Appointments (AppointmentID, PatientName, DoctorID, AppointmentDateTime, IsCancelled)
VALUES (1, 'Maxim Kauganau', 1, '2024-05-16 10:00:00', 0);