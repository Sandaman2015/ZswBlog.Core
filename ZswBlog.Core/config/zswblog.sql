/*
 Navicat Premium Data Transfer

 Source Server         : 本地
 Source Server Type    : MySQL
 Source Server Version : 80021
 Source Host           : localhost:3306
 Source Schema         : zswblog

 Target Server Type    : MySQL
 Target Server Version : 80021
 File Encoding         : 65001

 Date: 14/11/2020 11:49:27
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- ----------------------------
-- Table structure for tab_actionlog
-- ----------------------------
DROP TABLE IF EXISTS `tab_actionlog`;
CREATE TABLE `tab_actionlog`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '操作id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '操作人',
  `moduleName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '模块名称',
  `actionName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '方法名称',
  `actionUrl` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '操作地址',
  `ipAddress` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'IP地址',
  `logType` int(0) NOT NULL COMMENT '日志类型',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_announcement
-- ----------------------------
DROP TABLE IF EXISTS `tab_announcement`;
CREATE TABLE `tab_announcement`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '通知公告主键',
  `createDate` datetime(0) NOT NULL COMMENT '创建时间',
  `operatorId` int(0) NOT NULL COMMENT '发布人',
  `content` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NULL DEFAULT NULL COMMENT '发布内容',
  `isTop` smallint(0) NULL DEFAULT NULL COMMENT '是否置顶',
  `sort` int(0) NULL DEFAULT NULL COMMENT '置顶排序',
  `endPushDate` datetime(0) NOT NULL COMMENT '结束推送时间',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `Index_tab_annocement_id`(`endPushDate`) USING BTREE,
  INDEX `fk_tab_annocement_operatorId_tab_user_id`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_tab_annocement_operatorId_tab_user_id` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_0900_ai_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_article
-- ----------------------------
DROP TABLE IF EXISTS `tab_article`;
CREATE TABLE `tab_article`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '文章id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` int(0) NULL DEFAULT -1 COMMENT '操作人',
  `title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '文章标题',
  `content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '文章内容',
  `like` int(0) NOT NULL DEFAULT 0 COMMENT '点赞数',
  `categoryId` int(0) NOT NULL DEFAULT 0 COMMENT '所属分类，默认为0是默认分类',
  `visits` int(0) NOT NULL DEFAULT 0 COMMENT '浏览次数',
  `isShow` smallint(0) NOT NULL DEFAULT 0 COMMENT '是否显示0不显示,1显示',
  `lastUpdateDate` datetime(0) NULL DEFAULT NULL COMMENT '上次更新时间',
  `coverImage` varchar(300) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL COMMENT '文章插图',
  `isTop` int(0) NULL DEFAULT 1 COMMENT '是否置顶0不置顶,1置顶',
  `topSort` int(0) NULL DEFAULT -1 COMMENT '置顶排序',
  `readTime` smallint(0) NULL DEFAULT NULL COMMENT '阅读时间',
  `textCount` int(0) NULL DEFAULT NULL COMMENT '文章总字数',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_article_topSort`(`topSort`) USING BTREE,
  INDEX `index_article_category_categoryId`(`categoryId`) USING BTREE,
  CONSTRAINT `fk_article_category_categoryId` FOREIGN KEY (`categoryId`) REFERENCES `tab_category` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_category
-- ----------------------------
DROP TABLE IF EXISTS `tab_category`;
CREATE TABLE `tab_category`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '类型主键',
  `name` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '类型名称',
  `operatorId` int(0) NOT NULL COMMENT '操作人',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `description` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '副标题/描述',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_category_user_operatorId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_category_user_operatorId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_comment
-- ----------------------------
DROP TABLE IF EXISTS `tab_comment`;
CREATE TABLE `tab_comment`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '评论id',
  `createDate` datetime(0) NOT NULL COMMENT '创建时间',
  `content` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '评论内容',
  `userId` int(0) NOT NULL COMMENT '用户id',
  `articleId` int(0) NOT NULL COMMENT '所属文章id',
  `targetUserId` char(36) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '目标用户',
  `targetId` int(0) NULL DEFAULT 0 COMMENT '目标评论',
  `location` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '评论位置',
  `browser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '浏览器',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_comment_articleId`(`articleId`) USING BTREE,
  INDEX `index_comment_user_userId`(`userId`) USING BTREE,
  CONSTRAINT `fk_comment_article_articleId` FOREIGN KEY (`articleId`) REFERENCES `tab_article` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_comment_user_userId` FOREIGN KEY (`userId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_file_attachment
-- ----------------------------
DROP TABLE IF EXISTS `tab_file_attachment`;
CREATE TABLE `tab_file_attachment`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '附件id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '附件上传时间',
  `operatorId` int(0) NOT NULL DEFAULT -1 COMMENT '操作人',
  `fileName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '附件名称',
  `fileExt` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '附件后缀',
  `path` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '附件路径',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_file_attachment_user_userId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_file_attachment_user_userId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 19 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_friendlink
-- ----------------------------
DROP TABLE IF EXISTS `tab_friendlink`;
CREATE TABLE `tab_friendlink`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '友情链接id',
  `createDate` datetime(6) NOT NULL,
  `operatorId` int(0) NOT NULL COMMENT '操作人：外部操作绑定用户表',
  `title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '友情链接标题',
  `portrait` varchar(350) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '友情连接头像地址',
  `src` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '友情链接地址',
  `description` varchar(80) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '友情链接描述',
  `isShow` smallint(0) NULL DEFAULT 1 COMMENT '是否显示0不显示,1显示',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_friendlink_user_userId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_friendlink_user_userId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_message
-- ----------------------------
DROP TABLE IF EXISTS `tab_message`;
CREATE TABLE `tab_message`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '留言id',
  `content` varchar(500) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '留言内容',
  `createDate` datetime(0) NOT NULL COMMENT '留言时间',
  `userId` int(0) NOT NULL COMMENT '用户id',
  `targetUserId` int(0) NULL DEFAULT NULL COMMENT '目标留言用户',
  `targetId` int(0) NULL DEFAULT 0 COMMENT '目标留言id',
  `location` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '留言位置',
  `browser` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '浏览器',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_message_user_userId`(`userId`) USING BTREE,
  CONSTRAINT `fk_message_user_userId` FOREIGN KEY (`userId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 14 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_middle_article_tag
-- ----------------------------
DROP TABLE IF EXISTS `tab_middle_article_tag`;
CREATE TABLE `tab_middle_article_tag`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '中间表id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` int(0) NULL DEFAULT -1 COMMENT '操作人',
  `articleId` int(0) NOT NULL COMMENT '文章id',
  `tagId` int(0) NOT NULL COMMENT '标签id',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_article_tag_articleId`(`articleId`) USING BTREE,
  INDEX `index_article_tag_tagId`(`tagId`) USING BTREE,
  CONSTRAINT `fk_middle_article_tag_article_id` FOREIGN KEY (`articleId`) REFERENCES `tab_article` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_middle_article_tag_tag_id` FOREIGN KEY (`tagId`) REFERENCES `tab_tag` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_middle_travel_file_attachment
-- ----------------------------
DROP TABLE IF EXISTS `tab_middle_travel_file_attachment`;
CREATE TABLE `tab_middle_travel_file_attachment`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '中间表id',
  `createDate` datetime(0) NOT NULL COMMENT '创建时间',
  `operatorId` int(0) NOT NULL DEFAULT -1 COMMENT '操作人',
  `travelId` int(0) NOT NULL COMMENT '旅行分享id',
  `fileAttachmentId` int(0) NOT NULL COMMENT '上传附件id',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_travel_file_attachment_travelId`(`travelId`) USING BTREE,
  INDEX `index_travel_file_attachment_fileAttachmentId`(`fileAttachmentId`) USING BTREE,
  CONSTRAINT `fk_middle_file_attachment_id` FOREIGN KEY (`fileAttachmentId`) REFERENCES `tab_file_attachment` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT,
  CONSTRAINT `fk_middle_travel_id` FOREIGN KEY (`travelId`) REFERENCES `tab_travel` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_qq_userinfo
-- ----------------------------
DROP TABLE IF EXISTS `tab_qq_userinfo`;
CREATE TABLE `tab_qq_userinfo`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '自增主键',
  `openId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'QQ开放id',
  `accessToken` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'token',
  `gender` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '性别：默认男',
  `figureurl_qq_1` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '40*40的头像',
  `nickName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'QQ昵称',
  `userId` int(0) NOT NULL COMMENT '绑定用户id',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_qq_user_info_user_userId`(`userId`) USING BTREE,
  CONSTRAINT `fk_qq_user_info_user_userId` FOREIGN KEY (`userId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 3 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_sitetag
-- ----------------------------
DROP TABLE IF EXISTS `tab_sitetag`;
CREATE TABLE `tab_sitetag`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '站点标签id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` int(0) NOT NULL DEFAULT -1 COMMENT '操作人',
  `title` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '站点标签标题',
  `like` int(0) NULL DEFAULT 0 COMMENT '点赞数',
  `isShow` smallint(0) NOT NULL DEFAULT 1 COMMENT '是否显示0不显示,1显示',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_sitetag_user_userId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_sitetag_user_userId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB AUTO_INCREMENT = 1 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_tag
-- ----------------------------
DROP TABLE IF EXISTS `tab_tag`;
CREATE TABLE `tab_tag`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '标签id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` int(0) NOT NULL DEFAULT -1 COMMENT '操作人',
  `name` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '标签名称',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_tag_user_userId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_tag_user_userId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_timeline
-- ----------------------------
DROP TABLE IF EXISTS `tab_timeline`;
CREATE TABLE `tab_timeline`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '时间线id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` int(0) NOT NULL DEFAULT -1 COMMENT '操作人',
  `title` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '时间线标题',
  `content` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '时间线内容',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_timeline_user_userId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_timeline_user_userId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_travel
-- ----------------------------
DROP TABLE IF EXISTS `tab_travel`;
CREATE TABLE `tab_travel`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '旅行分享id',
  `createDate` datetime(0) NULL DEFAULT NULL COMMENT '创建时间',
  `operatorId` int(0) NOT NULL DEFAULT -1 COMMENT '操作人',
  `title` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '标题',
  `content` varchar(5000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '旅行分享内容',
  `isShow` smallint(0) NOT NULL DEFAULT 1 COMMENT '是否显示0不显示,1显示',
  `priview` varchar(255) CHARACTER SET utf8 COLLATE utf8_general_ci NULL DEFAULT NULL COMMENT '预览',
  PRIMARY KEY (`id`) USING BTREE,
  INDEX `index_travel_user_userId`(`operatorId`) USING BTREE,
  CONSTRAINT `fk_travel_user_userId` FOREIGN KEY (`operatorId`) REFERENCES `tab_user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE = InnoDB CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Table structure for tab_user
-- ----------------------------
DROP TABLE IF EXISTS `tab_user`;
CREATE TABLE `tab_user`  (
  `id` int(0) NOT NULL AUTO_INCREMENT COMMENT '用户id',
  `password` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '密码',
  `email` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '邮件地址',
  `nickName` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '昵称',
  `loginTime` datetime(6) NULL DEFAULT NULL COMMENT '首次登录',
  `portrait` varchar(400) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '头像',
  `createDate` datetime(0) NOT NULL COMMENT '创建时间',
  `lastLoginDate` datetime(0) NULL DEFAULT NULL COMMENT '上次登录',
  `loginCount` int(0) NULL DEFAULT 0 COMMENT '登录次数',
  `disabled` smallint(0) NOT NULL COMMENT '是否禁用 0禁用，1不禁用',
  PRIMARY KEY (`id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 6 CHARACTER SET = utf8 COLLATE = utf8_general_ci ROW_FORMAT = Dynamic;

SET FOREIGN_KEY_CHECKS = 1;
