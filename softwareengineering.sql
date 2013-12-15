-- phpMyAdmin SQL Dump
-- version 4.0.4
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2013 年 12 月 15 日 04:44
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
CREATE DATABASE IF NOT EXISTS `softwareengineering` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `softwareengineering`;

-- --------------------------------------------------------

--
-- 表的结构 `config_line_information`
--

CREATE TABLE IF NOT EXISTS `config_line_information` (
  `ID` int(11) NOT NULL,
  `line_name` text NOT NULL,
  `start_station_ID` text NOT NULL,
  `end_station_ID` text NOT NULL,
  `voltage_level` int(11) NOT NULL,
  `rated_current` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `config_station_information`
--

CREATE TABLE IF NOT EXISTS `config_station_information` (
  `ID` int(11) NOT NULL,
  `station_name` text NOT NULL,
  `longitude` float NOT NULL,
  `latitude` float NOT NULL,
  `builded_time` int(11) NOT NULL,
  `voltage_level` int(11) NOT NULL,
  `installed_capacity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `realtime_accident_log`
--

CREATE TABLE IF NOT EXISTS `realtime_accident_log` (
  `ID` int(11) NOT NULL,
  `breakdown_type` text NOT NULL,
  `timestamp` int(11) NOT NULL,
  `breakdown_information` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `realtime_debug_log`
--

CREATE TABLE IF NOT EXISTS `realtime_debug_log` (
  `ID` int(11) NOT NULL,
  `breakdown_ID` int(11) NOT NULL,
  `timestamp` int(11) NOT NULL,
  `isSolve` tinyint(1) NOT NULL,
  `solution_method` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `realtime_line_data`
--

CREATE TABLE IF NOT EXISTS `realtime_line_data` (
  `ID` int(11) NOT NULL,
  `line_ID` int(11) NOT NULL,
  `load` int(11) NOT NULL,
  `timestamp` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `realtime_station_data`
--

CREATE TABLE IF NOT EXISTS `realtime_station_data` (
  `ID` int(11) NOT NULL,
  `station_ID` int(11) NOT NULL,
  `active_power` int(11) NOT NULL,
  `reactive_power` int(11) NOT NULL,
  `time_stamp` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `realtime_user_lig`
--

CREATE TABLE IF NOT EXISTS `realtime_user_lig` (
  `ID` int(11) NOT NULL,
  `login_time` int(11) NOT NULL,
  `logout_time` int(11) NOT NULL,
  `user_ID` int(11) NOT NULL,
  `operation_list` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
