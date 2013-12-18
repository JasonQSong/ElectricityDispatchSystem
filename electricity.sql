-- phpMyAdmin SQL Dump
-- version 4.0.4
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2013 年 12 月 18 日 17:01
-- 服务器版本: 5.6.12-log
-- PHP 版本: 5.4.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 数据库: `electricity`
--
CREATE DATABASE IF NOT EXISTS `electricity` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `electricity`;

-- --------------------------------------------------------

--
-- 表的结构 `config_equipment`
--

CREATE TABLE IF NOT EXISTS `config_equipment` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '数据库自动为该设备生产的编码',
  `equipmentSN` int(11) NOT NULL COMMENT '该设备的唯一标示符',
  `Producer` varchar(50) NOT NULL COMMENT '该设备的生产商信息',
  `BoughtTime` datetime NOT NULL COMMENT '该设备买入的时间',
  `LifeTime` datetime NOT NULL COMMENT '该设备的寿命',
  `AccidentTimes` int(11) NOT NULL COMMENT '该设备发生故障的次数',
  `PersonID_InCharge` int(11) NOT NULL COMMENT '该设备的负责人',
  `StationID` int(11) NOT NULL COMMENT '设备所属的变电站的ID',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `config_line_information`
--

CREATE TABLE IF NOT EXISTS `config_line_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统为设备自动生成的编码',
  `LineName` varchar(20) NOT NULL COMMENT '输电线名称',
  `StationID_Start` int(11) NOT NULL COMMENT '输电线起点',
  `StationID_End` int(11) NOT NULL COMMENT '输电线终点',
  `VoltageLevel` varchar(20) NOT NULL COMMENT '电压等级',
  `RatedCurrent` varchar(20) NOT NULL COMMENT '额定功率',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `config_personinformation`
--

CREATE TABLE IF NOT EXISTS `config_personinformation` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的唯一标示符',
  `FullName` varchar(50) NOT NULL COMMENT '姓名',
  `Photo` varchar(50) NOT NULL COMMENT '照片的存储地址',
  `Birthday` datetime NOT NULL COMMENT '生日',
  `Gender` varchar(1) NOT NULL COMMENT '性别',
  `PhoneNumber` varchar(20) NOT NULL COMMENT '电话号码',
  `Address` text NOT NULL COMMENT '住址',
  `Post` varchar(50) NOT NULL COMMENT '职位',
  `Resume` text NOT NULL COMMENT '简历',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `config_station_information`
--

CREATE TABLE IF NOT EXISTS `config_station_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的编码',
  `StationName` varchar(50) NOT NULL COMMENT '变电站名称',
  `Longitude` double NOT NULL COMMENT '经度',
  `Latitude` double NOT NULL COMMENT '纬度',
  `BuildTime` datetime NOT NULL COMMENT '建造时间',
  `VoltageLevel` varchar(20) NOT NULL COMMENT '电压等级',
  `InstallCapacity` varchar(20) NOT NULL COMMENT '装机容量',
  `PersonID_InCharge` int(11) NOT NULL COMMENT '负责人ID（对应config_personinformation中的ID）',
  `BuildCompany` varchar(50) NOT NULL COMMENT '建造商',
  `AccidentTimes` int(11) NOT NULL COMMENT '发生故障的次数',
  `Address` text NOT NULL COMMENT '变电站地址',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `log_user`
--

CREATE TABLE IF NOT EXISTS `log_user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的编码',
  `Time` datetime NOT NULL COMMENT '操作时间',
  `UserID` int(11) NOT NULL COMMENT '操作者账号',
  `Action` text NOT NULL COMMENT '操作',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `runtime_accident`
--

CREATE TABLE IF NOT EXISTS `runtime_accident` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动为该记录生成的编码',
  `breakdown_type` text NOT NULL COMMENT '故障类型',
  `timestamp` int(11) NOT NULL COMMENT '故障时间',
  `breakdown_information` text NOT NULL COMMENT '故障信息',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `runtime_line_data`
--

CREATE TABLE IF NOT EXISTS `runtime_line_data` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的记录编码',
  `LineID` int(11) NOT NULL COMMENT '输电线编码',
  `LoadQuantity` varchar(20) NOT NULL COMMENT '负载',
  `Time` datetime NOT NULL COMMENT '时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `runtime_repair`
--

CREATE TABLE IF NOT EXISTS `runtime_repair` (
  `ID` int(11) NOT NULL COMMENT '记录编码',
  `breakdown_ID` int(11) NOT NULL COMMENT '故障编码',
  `timestamp` datetime NOT NULL COMMENT '时间戳',
  `isSolve` tinyint(1) NOT NULL COMMENT '故障是否排除',
  `solution_method` text NOT NULL COMMENT '故障排除方法',
  `PersonID_InCharge` int(11) NOT NULL COMMENT '负责修复的人的编码（对应于数config_personinformation的编码）'
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- 表的结构 `runtime_station_data`
--

CREATE TABLE IF NOT EXISTS `runtime_station_data` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '记录编码',
  `StationID` int(11) NOT NULL COMMENT '变电站编码',
  `ActivePower` varchar(20) NOT NULL COMMENT '有功功率',
  `ReactivePower` varchar(20) NOT NULL COMMENT '无功功率',
  `Time` datetime NOT NULL COMMENT '时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
