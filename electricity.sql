-- phpMyAdmin SQL Dump
-- version 4.0.4
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2013 年 12 月 19 日 09:02
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
  `EquipmentSN` varchar(20) NOT NULL DEFAULT 'EquipmentSN' COMMENT '该设备的唯一标示符',
  `Producer` varchar(50) NOT NULL DEFAULT 'Producer' COMMENT '该设备的生产商信息',
  `BoughtTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '该设备买入的时间',
  `LifeTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '该设备的寿命',
  `AccidentTimes` int(11) NOT NULL DEFAULT '0' COMMENT '该设备发生故障的次数',
  `PersonID_InCharge` int(11) NOT NULL DEFAULT '0' COMMENT '该设备的负责人',
  `StationID` int(11) NOT NULL DEFAULT '0' COMMENT '设备所属的变电站的ID',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `config_line_information`
--

CREATE TABLE IF NOT EXISTS `config_line_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统为设备自动生成的编码',
  `LineName` varchar(20) NOT NULL DEFAULT 'LineName' COMMENT '输电线名称',
  `StationID_Start` int(11) NOT NULL DEFAULT '0' COMMENT '输电线起点',
  `StationID_End` int(11) NOT NULL DEFAULT '0' COMMENT '输电线终点',
  `VoltageLevel` varchar(20) NOT NULL DEFAULT '-1' COMMENT '电压等级',
  `RatedCurrent` varchar(20) NOT NULL DEFAULT '-1' COMMENT '额定功率',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=30 ;

--
-- 转存表中的数据 `config_line_information`
--

INSERT INTO `config_line_information` (`ID`, `LineName`, `StationID_Start`, `StationID_End`, `VoltageLevel`, `RatedCurrent`) VALUES
(1, '中堡线', 3, 4, '220', '0'),
(2, '家堡线', 5, 4, '220', '0'),
(3, '海中线', 1, 3, '220', '0'),
(4, '中城线', 3, 7, '110', '0'),
(5, '中堡线2', 3, 4, '110', '0'),
(6, '中红线', 3, 6, '110', '0'),
(7, '中新线', 3, 8, '110', '0'),
(8, '中南线', 3, 7, '110', '0'),
(9, '堡竖线', 4, 10, '110', '0'),
(10, '堡长线', 4, 9, '110', '0'),
(11, '家汲线', 5, 11, '110', '0'),
(12, '家堡线2', 5, 4, '110', '0'),
(13, '家长线2', 5, 2, '110', '0'),
(14, '长江线', 9, 6, '110', '0'),
(15, '崇海线', 1, 8, '110', '0'),
(16, '双东线', 3, 26, '35', '0'),
(17, '双庙线', 3, 33, '35', '0'),
(18, '园区线', 3, 29, '35', '0'),
(19, '双明线', 3, 24, '35', '0'),
(20, '双建线', 3, 12, '35', '0'),
(21, '双湄线', 3, 18, '35', '0'),
(22, '双同线', 3, 13, '35', '0'),
(23, '堡滧线', 4, 35, '35', '0'),
(24, '堡镇线', 4, 15, '35', '0'),
(25, '堡船线', 4, 23, '35', '0'),
(26, '堡港线', 4, 32, '35', '0'),
(27, '家裕线', 5, 27, '35', '0'),
(28, '家前线', 5, 28, '35', '0'),
(29, '家陈线', 5, 16, '35', '0');

-- --------------------------------------------------------

--
-- 表的结构 `config_person_information`
--

CREATE TABLE IF NOT EXISTS `config_person_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的唯一标示符',
  `FullName` varchar(50) NOT NULL DEFAULT 'FullName' COMMENT '姓名',
  `Photo` varchar(50) NOT NULL DEFAULT 'default.jpg' COMMENT '照片的存储地址',
  `Birthday` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '生日',
  `Gender` varchar(20) NOT NULL DEFAULT 'male' COMMENT '性别',
  `PhoneNumber` varchar(20) NOT NULL DEFAULT '00000000' COMMENT '电话号码',
  `Address` text NOT NULL COMMENT '住址',
  `Post` varchar(50) NOT NULL DEFAULT 'Post' COMMENT '职位',
  `Resume` text NOT NULL COMMENT '简历',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `config_station_information`
--

CREATE TABLE IF NOT EXISTS `config_station_information` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的编码',
  `StationName` varchar(50) NOT NULL DEFAULT 'StationName' COMMENT '变电站名称',
  `Longitude` varchar(20) NOT NULL DEFAULT '0' COMMENT '经度',
  `Latitude` varchar(20) NOT NULL DEFAULT '0' COMMENT '纬度',
  `BuildTime` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '建造时间',
  `VoltageLevel` varchar(20) NOT NULL DEFAULT '-1' COMMENT '电压等级',
  `InstallCapacity` varchar(20) NOT NULL DEFAULT '-1' COMMENT '装机容量',
  `PersonID_InCharge` int(11) NOT NULL DEFAULT '0' COMMENT '负责人ID（对应config_personinformation中的ID）',
  `BuildCompany` varchar(50) NOT NULL DEFAULT 'BuildCompany' COMMENT '建造商',
  `AccidentTimes` int(11) NOT NULL DEFAULT '0' COMMENT '发生故障的次数',
  `Address` text NOT NULL COMMENT '变电站地址',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=36 ;

--
-- 转存表中的数据 `config_station_information`
--

INSERT INTO `config_station_information` (`ID`, `StationName`, `Longitude`, `Latitude`, `BuildTime`, `VoltageLevel`, `InstallCapacity`, `PersonID_InCharge`, `BuildCompany`, `AccidentTimes`, `Address`) VALUES
(1, '南通海门变', '121.182', '31.902', '0001-01-01 12:00:00', '220', '0', 0, 'BuildCompany', 0, ''),
(2, '长兴站', '121.697', '31.39', '0001-01-01 12:00:00', '220', '0', 0, 'BuildCompany', 0, ''),
(3, '中双港站', '121.405', '31.662', '0001-01-01 12:00:00', '220', '0', 0, 'BuildCompany', 0, ''),
(4, '堡北站', '121.631', '31.572', '0001-01-01 12:00:00', '220', '0', 0, 'BuildCompany', 0, ''),
(5, '陈家镇站', '121.81', '31.498', '0001-01-01 12:00:00', '220', '0', 0, 'BuildCompany', 0, ''),
(6, '红星站', '121.372', '31.787', '0001-01-01 12:00:00', '110', '0', 0, 'BuildCompany', 0, ''),
(7, '南门站', '121.394', '31.618', '0001-01-01 12:00:00', '110', '0', 0, 'BuildCompany', 0, ''),
(8, '新海站', '121.297', '31.819', '0001-01-01 12:00:00', '110', '0', 0, 'BuildCompany', 0, ''),
(9, '长江站', '121.539', '31.678', '0001-01-01 12:00:00', '110', '0', 0, 'BuildCompany', 0, ''),
(10, '竖河站', '121.591', '31.602', '0001-01-01 12:00:00', '110', '0', 0, 'BuildCompany', 0, ''),
(11, '汲浜站', '121.761', '31.533', '0001-01-01 12:00:00', '110', '0', 0, 'BuildCompany', 0, ''),
(12, '建设站', '121.456', '31.655', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(13, '大同站', '121.489', '31.627', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(14, '新河站', '121.525', '31.583', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(15, '堡镇站', '121.619', '31.537', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(16, '陈东站', '121.82', '31.497', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(17, '森林站', '121.479', '31.677', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(18, '湄洲站', '121.408', '31.62', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(19, '崇安站', '121.341', '31.664', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(20, '新闸站', '121.381', '31.635', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(21, '跃进站', '121.225', '31.813', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(22, '富盛站', '121.517', '31.589', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(23, '上船站', '121.555', '31.562', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(24, '明珠站', '121.412', '31.638', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(25, '三星站', '121.288', '31.744', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(26, '东风站', '121.491', '31.712', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(27, '裕安站', '121.831', '31.524', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(28, '前哨站', '121.87', '31.526', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(29, '园区站', '121.396', '31.636', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(30, '启隆站', '121.456', '31.775', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(31, '前进站', '121.6', '31.643', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(32, '港沿站', '121.657', '31.589', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(33, '庙镇站', '121.35', '31.714', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(34, '前卫站', '121.512', '31.719', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, ''),
(35, '五滧站', '121.664', '31.54', '0001-01-01 12:00:00', '35', '0', 0, 'BuildCompany', 0, '');

-- --------------------------------------------------------

--
-- 表的结构 `log_user`
--

CREATE TABLE IF NOT EXISTS `log_user` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的编码',
  `Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '操作时间',
  `UserID` int(11) NOT NULL DEFAULT '0' COMMENT '操作者账号',
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
  `timestamp` int(11) NOT NULL DEFAULT '0' COMMENT '故障时间',
  `breakdown_information` text NOT NULL COMMENT '故障信息',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `runtime_line_data`
--

CREATE TABLE IF NOT EXISTS `runtime_line_data` (
  `ID` int(11) NOT NULL AUTO_INCREMENT COMMENT '系统自动生成的记录编码',
  `LineID` int(11) NOT NULL DEFAULT '0' COMMENT '输电线编码',
  `LoadQuantity` varchar(20) NOT NULL DEFAULT '-1' COMMENT '负载',
  `Time` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '时间',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- 表的结构 `runtime_repair`
--

CREATE TABLE IF NOT EXISTS `runtime_repair` (
  `ID` int(11) NOT NULL COMMENT '记录编码',
  `breakdown_ID` int(11) NOT NULL DEFAULT '0' COMMENT '故障编码',
  `timestamp` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' COMMENT '时间戳',
  `isSolve` tinyint(1) NOT NULL DEFAULT '0' COMMENT '故障是否排除',
  `solution_method` text NOT NULL COMMENT '故障排除方法',
  `PersonID_InCharge` int(11) NOT NULL DEFAULT '0' COMMENT '负责修复的人的编码（对应于数config_personinformation的编码）'
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
