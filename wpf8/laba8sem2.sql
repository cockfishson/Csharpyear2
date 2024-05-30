CREATE DATABASE ApartmentDB;
GO

USE ApartmentDB;
GO

CREATE TABLE Apartment (
    ApartmentID INT PRIMARY KEY IDENTITY,
    Area DECIMAL(10,2),
    Rooms INT,
    Kitchen BIT,
    Bathroom BIT,
    Toilet BIT,
    Basement BIT,
    Balcony BIT,
    ConstructionYear INT,
    MaterialType VARCHAR(50),
    Floor INT,
    Photo IMAGE 
);
GO

CREATE TABLE Address (
    AddressID INT PRIMARY KEY IDENTITY,
    Country VARCHAR(100),
    City VARCHAR(100),
    District VARCHAR(100),
    Street VARCHAR(100),
    HouseNumber VARCHAR(20),
    Building VARCHAR(20),
    ApartmentNumber VARCHAR(20)
);
GO

CREATE TABLE Room (
    RoomID INT PRIMARY KEY IDENTITY,
    Area DECIMAL(10,2),
    WindowsCount INT,
    WindowDirection VARCHAR(20), 
    ApartmentID INT FOREIGN KEY REFERENCES Apartment(ApartmentID)
);
GO

CREATE TRIGGER RoomAreaCheck
ON Room
AFTER INSERT
AS
BEGIN
    DECLARE @ApartmentID INT;
    DECLARE @ApartmentArea DECIMAL(10,2);
    SELECT @ApartmentID = i.ApartmentID,
           @ApartmentArea = a.Area
    FROM inserted i
    INNER JOIN Apartment a ON i.ApartmentID = a.ApartmentID;
    IF (SELECT SUM(Area) FROM Room WHERE ApartmentID = @ApartmentID) > @ApartmentArea
    BEGIN
        RAISERROR ('Total room area cannot exceed apartment area', 16, 1)
        ROLLBACK TRANSACTION;
    END
END;


CREATE PROCEDURE AddApartment
    @Area DECIMAL(10,2),
    @Rooms INT,
    @Kitchen BIT,
    @Bathroom BIT,
    @Toilet BIT,
    @Basement BIT,
    @Balcony BIT,
    @ConstructionYear INT,
    @MaterialType VARCHAR(50),
    @Floor INT,
    @Photo IMAGE,
    @Country VARCHAR(100),
    @City VARCHAR(100),
    @District VARCHAR(100),
    @Street VARCHAR(100),
    @HouseNumber VARCHAR(20),
    @Building VARCHAR(20),
    @ApartmentNumber VARCHAR(20)
AS
BEGIN
    INSERT INTO Address (Country, City, District, Street, HouseNumber, Building, ApartmentNumber)
    VALUES (@Country, @City, @District, @Street, @HouseNumber, @Building, @ApartmentNumber);

    DECLARE @AddressID INT;
    SET @AddressID = SCOPE_IDENTITY();

    INSERT INTO Apartment (Area, Rooms, Kitchen, Bathroom, Toilet, Basement, Balcony, ConstructionYear, MaterialType, Floor, Photo)
    VALUES (@Area, @Rooms, @Kitchen, @Bathroom, @Toilet, @Basement, @Balcony, @ConstructionYear, @MaterialType, @Floor, @Photo);

    DECLARE @ApartmentID INT;
    SET @ApartmentID = SCOPE_IDENTITY();

    UPDATE Room SET ApartmentID = @ApartmentID WHERE ApartmentID IS NULL;

END;
GO

INSERT INTO Apartment (Area, Rooms, Kitchen, Bathroom, Toilet, Basement, Balcony, ConstructionYear, MaterialType, Floor, Photo,Adress)
VALUES
(70.5, 3, 1, 1, 1, 0, 1, 2005, 'Brick', 4, NULL,1),
(45.2, 2, 1, 1, 1, 0, 0, 2010, 'Panel', 9, NULL,2),
(30.0, 1, 1, 2, 2, 1, 1, 2018, 'Brick', 2, NULL,3);

INSERT INTO Address (Country, City, District, Street, HouseNumber, Building, ApartmentNumber)
VALUES
('Belarus', 'Minsk', 'Central', 'Lenina', '10', '1', '5'),
('Belarus', 'Minsk', 'Sovetskiy', 'Pushkina', '15', '2', '10'),
('Belarus', 'Minsk', 'Zavodskoy', 'Gorkogo', '5', '5', '25');

INSERT INTO Room (Area, WindowsCount, WindowDirection, ApartmentID)
VALUES
(30.0, 1, 'East', 2),
(20.5, 1, 'South', 2),
(20.0, 2, 'West', 2),
(20.0, 1, 'East', 3),
(25.2, 1, 'South', 3),
(30.0, 2, 'West', 4);
