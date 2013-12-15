-- phpMyAdmin SQL Dump
-- version 4.0.4
-- http://www.phpmyadmin.net
--
-- 主机: localhost
-- 生成日期: 2013 年 12 月 15 日 01:17
-- 服务器版本: 5.6.12-log
-- PHP 版本: 5.4.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- 数据库: `realtime_data`
--
CREATE DATABASE IF NOT EXISTS `realtime_data` DEFAULT CHARACTER SET latin1 COLLATE latin1_swedish_ci;
USE `realtime_data`;

-- --------------------------------------------------------

--
-- 表的结构 `accident_log`
--

CREATE TABLE IF NOT EXISTS `accident_log` (
  `ID` int(11) NOT NULL,
  `breakdown_type` text NOT NULL,
  `timestamp` int(11) NOT NULL,
  `breakdown_information` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `debug_log`
--

CREATE TABLE IF NOT EXISTS `debug_log` (
  `ID` int(11) NOT NULL,
  `breakdown_ID` int(11) NOT NULL,
  `timestamp` int(11) NOT NULL,
  `isSolve` tinyint(1) NOT NULL,
  `solution_method` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `line_data`
--

CREATE TABLE IF NOT EXISTS `line_data` (
  `ID` int(11) NOT NULL,
  `line_ID` int(11) NOT NULL,
  `load` int(11) NOT NULL,
  `date_stamp` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `station_data`
--

CREATE TABLE IF NOT EXISTS `station_data` (
  `ID` int(11) NOT NULL,
  `station_ID` int(11) NOT NULL,
  `active_power` int(11) NOT NULL,
  `reactive_power` int(11) NOT NULL,
  `time_stamp` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- 表的结构 `user_log`
--

CREATE TABLE IF NOT EXISTS `user_log` (
  `ID` int(11) NOT NULL,
  `login_time` int(11) NOT NULL,
  `logout_time` int(11) NOT NULL,
  `user_ID` int(11) NOT NULL,
  `operation_list` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
