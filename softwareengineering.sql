-- phpMyAdmin SQL Dump
-- version 4.0.4
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2013 年 12 月 18 日 13:47
-- 服务器版本: 5.6.12-log
-- PHP 版本: 5.4.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 数据库: `softwareengineering`
--
CREATE DATABASE IF NOT EXISTS `softwareengineering` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `softwareengineering`;

-- --------------------------------------------------------

--
-- 表的结构 `config_line_information`
--

CREATE TABLE IF NOT EXISTS `config_line_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `LineName` varchar(20) NOT NULL,
  `StationID_Start` int(11) NOT NULL,
  `StationID_End` int(11) NOT NULL,
  `VoltageLevel` varchar(20) NOT NULL,
  `RatedCurrent` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
);

-- --------------------------------------------------------

--
-- 表的结构 `config_station_information`
--

CREATE TABLE IF NOT EXISTS `config_station_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StationName` varchar(50) NOT NULL,
  `Longitude` double NOT NULL,
  `Latitude` double NOT NULL,
  `BuildTime` datetime NOT NULL,
  `VoltageLevel` varchar(20) NOT NULL,
  `InstallCapacity` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
);

CREATE TABLE IF NOT EXISTS `config_station_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `EqSN` varchar(50) NOT NULL,
  `StationID` double NOT NULL,
  `Description` double NOT NULL,
  `EqName` datetime NOT NULL,
  `VoltageLevel` varchar(20) NOT NULL,
  `InstallCapacity` varchar(20) NOT NULL,
  PRIMARY KEY (`ID`)
);
-- --------------------------------------------------------

--
-- 表的结构 `log_user`
--

CREATE TABLE IF NOT EXISTS `log_user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Time` datetime NOT NULL,
  `UserID` int(11) NOT NULL,
  `Action` text NOT NULL,
  PRIMARY KEY (`ID`)
);

-- --------------------------------------------------------

--
-- 表的结构 `runtime_accident`
--

CREATE TABLE IF NOT EXISTS `runtime_accident` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `breakdown_type` text NOT NULL,
  `timestamp` int(11) NOT NULL,
  `breakdown_information` text NOT NULL,
  PRIMARY KEY (`ID`)
);

-- --------------------------------------------------------

--
-- 表的结构 `runtime_line_data`
--

CREATE TABLE IF NOT EXISTS `runtime_line_data` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `LineID` int(11) NOT NULL,
  `LoadQuantity` varchar(20) NOT NULL,
  `Time` datetime NOT NULL,
  PRIMARY KEY (`ID`)
);

-- --------------------------------------------------------

--
-- 表的结构 `runtime_repair`
--

CREATE TABLE IF NOT EXISTS `runtime_repair` (
  `ID` int(11) NOT NULL,
  `breakdown_ID` int(11) NOT NULL,
  `timestamp` int(11) NOT NULL,
  `isSolve` tinyint(1) NOT NULL,
  `solution_method` text NOT NULL
);

-- --------------------------------------------------------

--
-- 表的结构 `runtime_station_data`
--

CREATE TABLE IF NOT EXISTS `runtime_station_data` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `StationID` int(11) NOT NULL,
  `ActivePower` varchar(20) NOT NULL,
  `ReactivePower` varchar(20) NOT NULL,
  `Time` datetime NOT NULL,
  PRIMARY KEY (`ID`)
);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
