-- phpMyAdmin SQL Dump
-- version 4.4.15.10
-- https://www.phpmyadmin.net
--
-- Host: localhost
-- Generation Time: 2021-06-10 12:22:52
-- 服务器版本： 5.7.27
-- PHP Version: 5.3.29

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `coreshopmysql`
--

-- --------------------------------------------------------

--
-- 表的结构 `corecmsadvertisement`
--

CREATE TABLE IF NOT EXISTS `corecmsadvertisement` (
  `id` int(11) NOT NULL COMMENT '序列',
  `positionId` int(11) NOT NULL COMMENT '位置序列',
  `name` varchar(50) NOT NULL COMMENT '广告名称',
  `imageUrl` varchar(255) NOT NULL COMMENT '广告图片id',
  `val` varchar(255) NOT NULL COMMENT '属性值',
  `valDes` longtext COMMENT '属性值说明',
  `sort` int(11) NOT NULL COMMENT '排序',
  `createTime` datetime DEFAULT NULL COMMENT '添加时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `code` varchar(32) DEFAULT NULL COMMENT '广告位置编码',
  `type` int(11) NOT NULL COMMENT '类型'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='广告表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsadvertposition`
--

CREATE TABLE IF NOT EXISTS `corecmsadvertposition` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(120) NOT NULL COMMENT '名称',
  `code` varchar(32) NOT NULL COMMENT '位置编码',
  `createTime` datetime DEFAULT NULL COMMENT '添加时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isEnable` bit(1) NOT NULL COMMENT '是否启用',
  `sort` int(11) NOT NULL COMMENT '排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='广告位置表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsagent`
--

CREATE TABLE IF NOT EXISTS `corecmsagent` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户Id',
  `name` varchar(255) DEFAULT NULL COMMENT '代理商名称',
  `gradeId` int(11) NOT NULL COMMENT '代理商等级',
  `mobile` varchar(50) DEFAULT NULL COMMENT '手机号',
  `weixin` varchar(50) DEFAULT NULL COMMENT '微信号',
  `qq` varchar(50) DEFAULT NULL COMMENT 'qq号',
  `storeName` varchar(255) DEFAULT NULL COMMENT '店铺名称',
  `storeLogo` varchar(255) DEFAULT NULL COMMENT '店铺Logo',
  `storeBanner` varchar(255) DEFAULT NULL COMMENT '店铺Banner',
  `storeDesc` varchar(255) DEFAULT NULL COMMENT '店铺简介',
  `verifyStatus` int(11) NOT NULL COMMENT '审核状态',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `verifyTime` datetime DEFAULT NULL COMMENT '审核时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='代理商表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsagentgoods`
--

CREATE TABLE IF NOT EXISTS `corecmsagentgoods` (
  `id` int(11) NOT NULL COMMENT '序列',
  `goodId` int(11) NOT NULL COMMENT '商品序列',
  `goodRefreshTime` datetime DEFAULT NULL COMMENT '商品编辑时间',
  `sortId` int(11) NOT NULL COMMENT '排序',
  `isEnable` bit(1) NOT NULL COMMENT '是否启用',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '最后更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='代理商品池';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsagentgrade`
--

CREATE TABLE IF NOT EXISTS `corecmsagentgrade` (
  `id` int(11) NOT NULL COMMENT '等级序列',
  `name` varchar(50) DEFAULT NULL COMMENT '等级名称',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认等级',
  `isAutoUpGrade` bit(1) NOT NULL COMMENT '是否自动升级',
  `defaultSalesPriceType` int(11) NOT NULL COMMENT '价格加成方式',
  `defaultSalesPriceNumber` int(11) NOT NULL COMMENT '价格加成值',
  `sortId` int(11) NOT NULL COMMENT '等级排序',
  `description` varchar(500) DEFAULT NULL COMMENT '等级说明'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='代理商等级设置表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsagentorder`
--

CREATE TABLE IF NOT EXISTS `corecmsagentorder` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户代理商id',
  `buyUserId` int(11) NOT NULL COMMENT '下单用户id',
  `orderId` varchar(50) DEFAULT NULL COMMENT '订单编号',
  `amount` decimal(10,0) NOT NULL COMMENT '结算金额',
  `isSettlement` int(11) NOT NULL COMMENT '是否结算',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='代理商订单记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsagentproducts`
--

CREATE TABLE IF NOT EXISTS `corecmsagentproducts` (
  `id` int(11) NOT NULL COMMENT '序列',
  `agentGoodsId` int(11) NOT NULL COMMENT '关联代理商品池',
  `goodId` int(11) NOT NULL COMMENT '商品序列',
  `productId` int(11) NOT NULL COMMENT '货品序列',
  `productCostPrice` decimal(10,0) NOT NULL COMMENT '货品成本价格',
  `productPrice` decimal(10,0) NOT NULL COMMENT '货品销售价格',
  `agentGradeId` int(11) NOT NULL COMMENT '代理商等级',
  `agentGradePrice` decimal(10,0) NOT NULL COMMENT '代理价格',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDel` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='代理货品池';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsapiaccesstoken`
--

CREATE TABLE IF NOT EXISTS `corecmsapiaccesstoken` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(50) NOT NULL COMMENT '第三方应用名称',
  `code` varchar(50) NOT NULL COMMENT '第三方应用编码',
  `machineCode` varchar(50) NOT NULL COMMENT '易联云终端号',
  `accessToken` varchar(50) NOT NULL COMMENT '访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存',
  `refreshToken` varchar(50) DEFAULT NULL COMMENT '更新access_token所需，有效时间35天',
  `expiresIn` int(11) NOT NULL COMMENT '令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数',
  `expiressEndTime` datetime DEFAULT NULL COMMENT '有效期截止时间',
  `parameters` longtext COMMENT '其他参数',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='第三方授权记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsarea`
--

CREATE TABLE IF NOT EXISTS `corecmsarea` (
  `id` int(11) NOT NULL COMMENT '地区ID',
  `parentId` int(11) DEFAULT NULL COMMENT '父级ID',
  `depth` int(11) DEFAULT NULL COMMENT '地区深度',
  `name` varchar(50) DEFAULT NULL COMMENT '地区名称',
  `postalCode` varchar(10) DEFAULT NULL COMMENT '邮编',
  `sort` int(11) NOT NULL COMMENT '地区排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='地区表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsarticle`
--

CREATE TABLE IF NOT EXISTS `corecmsarticle` (
  `id` int(11) NOT NULL COMMENT '序列',
  `title` varchar(200) NOT NULL COMMENT '标题',
  `brief` varchar(100) DEFAULT NULL COMMENT '简介',
  `coverImage` varchar(255) DEFAULT NULL COMMENT '封面图',
  `contentBody` longtext NOT NULL COMMENT '文章内容',
  `typeId` int(11) NOT NULL COMMENT '分类id',
  `sort` int(11) NOT NULL COMMENT '排序',
  `isPub` bit(1) NOT NULL COMMENT '是否发布',
  `isDel` bit(1) DEFAULT NULL COMMENT '是否删除',
  `pv` int(11) NOT NULL COMMENT '访问量',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='文章表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsarticletype`
--

CREATE TABLE IF NOT EXISTS `corecmsarticletype` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(32) NOT NULL COMMENT '分类名称',
  `parentId` int(11) NOT NULL COMMENT '父id',
  `sort` int(11) NOT NULL COMMENT '排序 '
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='文章分类表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillaftersales`
--

CREATE TABLE IF NOT EXISTS `corecmsbillaftersales` (
  `aftersalesId` varchar(20) NOT NULL COMMENT '售后单id',
  `orderId` varchar(20) NOT NULL COMMENT '订单ID',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `type` int(11) NOT NULL COMMENT '售后类型',
  `refundAmount` decimal(10,0) NOT NULL COMMENT '退款金额',
  `status` int(11) NOT NULL COMMENT '状态',
  `reason` varchar(255) NOT NULL COMMENT '退款原因',
  `mark` varchar(255) DEFAULT NULL COMMENT '卖家备注，如果审核失败了，会显示到前端',
  `createTime` datetime NOT NULL COMMENT '提交时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='退货单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillaftersalesimages`
--

CREATE TABLE IF NOT EXISTS `corecmsbillaftersalesimages` (
  `aftersalesId` varchar(20) NOT NULL COMMENT '售后单id',
  `imageUrl` varchar(255) NOT NULL COMMENT '图片地址',
  `sortId` int(11) NOT NULL COMMENT '排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品图片关联表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillaftersalesitem`
--

CREATE TABLE IF NOT EXISTS `corecmsbillaftersalesitem` (
  `id` int(11) NOT NULL COMMENT '序列',
  `aftersalesId` varchar(32) NOT NULL COMMENT '售后单id',
  `orderItemsId` int(11) NOT NULL COMMENT '订单明细ID 关联order_items.id',
  `goodsId` int(11) NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int(11) NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(30) DEFAULT NULL COMMENT '货品编码',
  `bn` varchar(30) DEFAULT NULL COMMENT '商品编码',
  `name` varchar(200) DEFAULT NULL COMMENT '商品名称',
  `imageUrl` varchar(255) NOT NULL COMMENT '图片',
  `nums` int(11) NOT NULL COMMENT '数量',
  `addon` longtext COMMENT '货品明细序列号存储',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='售后单明细表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbilldelivery`
--

CREATE TABLE IF NOT EXISTS `corecmsbilldelivery` (
  `deliveryId` varchar(20) NOT NULL COMMENT '发货单序列',
  `orderId` varchar(500) DEFAULT NULL COMMENT '订单号',
  `logiCode` varchar(50) DEFAULT NULL COMMENT '物流公司编码',
  `logiNo` varchar(50) DEFAULT NULL COMMENT '物流单号',
  `logiInformation` longtext COMMENT '快递物流信息',
  `logiStatus` bit(1) NOT NULL COMMENT '快递是否不更新',
  `shipAreaId` int(11) NOT NULL COMMENT '收货地区ID',
  `shipAddress` varchar(200) DEFAULT NULL COMMENT '收货详细地址',
  `shipName` varchar(50) DEFAULT NULL COMMENT '收货人姓名',
  `shipMobile` varchar(50) DEFAULT NULL COMMENT '收货电话',
  `status` int(11) NOT NULL COMMENT '状态',
  `memo` varchar(255) DEFAULT NULL COMMENT '备注',
  `confirmTime` datetime DEFAULT NULL COMMENT '确认收货时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='发货单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbilldeliveryitem`
--

CREATE TABLE IF NOT EXISTS `corecmsbilldeliveryitem` (
  `id` int(11) NOT NULL COMMENT '序列',
  `deliveryId` varchar(20) NOT NULL COMMENT '发货单号 关联bill_delivery.id',
  `goodsId` int(11) NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int(11) NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(30) NOT NULL COMMENT '货品编码',
  `bn` varchar(30) NOT NULL COMMENT '商品编码',
  `name` varchar(200) NOT NULL COMMENT '商品名称',
  `nums` int(11) NOT NULL COMMENT '发货数量',
  `weight` decimal(10,0) NOT NULL COMMENT '重量',
  `addon` longtext COMMENT '货品明细序列号存储'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='发货单详情表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbilldeliveryorderrel`
--

CREATE TABLE IF NOT EXISTS `corecmsbilldeliveryorderrel` (
  `id` int(11) NOT NULL COMMENT 'ID',
  `orderId` varchar(20) NOT NULL COMMENT '订单号',
  `deliveryId` varchar(20) NOT NULL COMMENT '发货单号'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='发货单订单关联表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbilllading`
--

CREATE TABLE IF NOT EXISTS `corecmsbilllading` (
  `id` varchar(20) NOT NULL COMMENT '提货单号',
  `orderId` varchar(20) NOT NULL COMMENT '订单号',
  `storeId` int(11) NOT NULL COMMENT '提货门店ID',
  `name` varchar(30) DEFAULT NULL COMMENT '提货人姓名',
  `mobile` varchar(15) DEFAULT NULL COMMENT '提货手机号',
  `clerkId` int(11) NOT NULL COMMENT '处理店员ID',
  `pickUpTime` datetime DEFAULT NULL COMMENT '提货时间',
  `status` bit(1) NOT NULL COMMENT '是否提货',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDel` bit(1) NOT NULL COMMENT '删除时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='提货单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillpayments`
--

CREATE TABLE IF NOT EXISTS `corecmsbillpayments` (
  `paymentId` varchar(20) NOT NULL COMMENT '支付单号',
  `money` decimal(10,0) NOT NULL COMMENT '支付金额',
  `userId` int(11) NOT NULL COMMENT '用户ID 关联user.id',
  `type` int(11) NOT NULL COMMENT '单据类型',
  `status` int(11) NOT NULL COMMENT '支付状态',
  `paymentCode` varchar(50) NOT NULL COMMENT '支付类型编码 关联payments.code',
  `ip` varchar(50) NOT NULL COMMENT '支付单生成IP',
  `parameters` varchar(200) DEFAULT NULL COMMENT '支付的时候需要的参数，存的是json格式的一维数组',
  `payedMsg` varchar(255) DEFAULT NULL COMMENT '支付回调后的状态描述',
  `tradeNo` varchar(50) DEFAULT NULL COMMENT '第三方平台交易流水号',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='支付单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillpaymentsrel`
--

CREATE TABLE IF NOT EXISTS `corecmsbillpaymentsrel` (
  `paymentId` varchar(20) NOT NULL COMMENT '支付单编号',
  `sourceId` varchar(20) NOT NULL COMMENT '资源编号',
  `money` decimal(10,0) NOT NULL COMMENT '金额'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='支付单明细表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillrefund`
--

CREATE TABLE IF NOT EXISTS `corecmsbillrefund` (
  `refundId` varchar(20) NOT NULL COMMENT '退款单ID',
  `aftersalesId` varchar(20) NOT NULL COMMENT '售后单id',
  `money` decimal(10,0) NOT NULL COMMENT '退款金额',
  `userId` int(11) NOT NULL COMMENT '用户ID 关联user.id',
  `sourceId` varchar(20) NOT NULL COMMENT '资源id，根据type不同而关联不同的表',
  `type` int(11) NOT NULL COMMENT '资源类型1=订单,2充值单',
  `paymentCode` varchar(50) DEFAULT NULL COMMENT '退款支付类型编码 默认原路返回 关联支付单表支付编码',
  `tradeNo` varchar(50) DEFAULT NULL COMMENT '第三方平台交易流水号',
  `status` int(11) NOT NULL COMMENT '状态',
  `memo` longtext COMMENT '退款失败原因',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='退款单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillreship`
--

CREATE TABLE IF NOT EXISTS `corecmsbillreship` (
  `reshipId` varchar(20) NOT NULL COMMENT '退货单号',
  `orderId` varchar(20) NOT NULL COMMENT '订单序列',
  `aftersalesId` varchar(20) NOT NULL COMMENT '售后单序列',
  `userId` int(11) NOT NULL COMMENT '用户ID 关联user.id',
  `logiCode` varchar(50) DEFAULT NULL COMMENT '物流公司编码',
  `logiNo` varchar(50) DEFAULT NULL COMMENT '物流单号',
  `status` int(11) NOT NULL COMMENT '状态',
  `memo` varchar(255) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='退货单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbillreshipitem`
--

CREATE TABLE IF NOT EXISTS `corecmsbillreshipitem` (
  `id` int(11) NOT NULL COMMENT '序列',
  `reshipId` varchar(20) NOT NULL COMMENT '退款单单id',
  `orderItemsId` int(11) NOT NULL COMMENT '订单明细ID 关联order_items.id',
  `goodsId` int(11) NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int(11) NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(30) DEFAULT NULL COMMENT '货品编码',
  `bn` varchar(30) DEFAULT NULL COMMENT '商品编码',
  `name` varchar(200) DEFAULT NULL COMMENT '商品名称',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '图片',
  `nums` int(11) NOT NULL COMMENT '数量',
  `addon` longtext COMMENT '货品明细序列号存储',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='退货单明细表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsbrand`
--

CREATE TABLE IF NOT EXISTS `corecmsbrand` (
  `id` int(11) NOT NULL COMMENT '品牌ID',
  `name` varchar(50) DEFAULT NULL COMMENT '品牌名称',
  `logoImageUrl` varchar(255) DEFAULT NULL COMMENT '品牌LOGO',
  `sort` int(11) DEFAULT NULL COMMENT '品牌排序',
  `isShow` bit(1) NOT NULL COMMENT '是否显示',
  `createTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='品牌表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmscart`
--

CREATE TABLE IF NOT EXISTS `corecmscart` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户序列',
  `productId` int(11) NOT NULL COMMENT '货品序列',
  `nums` int(11) NOT NULL COMMENT '货品数量',
  `type` int(11) NOT NULL COMMENT '购物车类型'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='购物车表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsclerk`
--

CREATE TABLE IF NOT EXISTS `corecmsclerk` (
  `id` int(11) NOT NULL COMMENT '序列',
  `storeId` int(11) NOT NULL COMMENT '店铺ID',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '删除时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='店铺店员关联表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmscoupon`
--

CREATE TABLE IF NOT EXISTS `corecmscoupon` (
  `id` int(11) NOT NULL COMMENT '序列',
  `couponCode` varchar(20) NOT NULL COMMENT '优惠券编码',
  `promotionId` int(11) NOT NULL COMMENT '优惠券id',
  `isUsed` bit(1) NOT NULL COMMENT '是否使用',
  `userId` int(11) NOT NULL COMMENT '谁领取了',
  `usedId` varchar(50) DEFAULT NULL COMMENT '被谁用了',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `remark` varchar(500) DEFAULT NULL COMMENT '说明',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='优惠券表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsdistribution`
--

CREATE TABLE IF NOT EXISTS `corecmsdistribution` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户Id',
  `name` varchar(255) DEFAULT NULL COMMENT '分销商名称',
  `gradeId` int(11) NOT NULL COMMENT '分销等级',
  `mobile` varchar(50) DEFAULT NULL COMMENT '手机号',
  `weixin` varchar(50) DEFAULT NULL COMMENT '微信号',
  `qq` varchar(50) DEFAULT NULL COMMENT 'qq号',
  `storeName` varchar(255) DEFAULT NULL COMMENT '店铺名称',
  `storeLogo` varchar(255) DEFAULT NULL COMMENT '店铺Logo',
  `storeBanner` varchar(255) DEFAULT NULL COMMENT '店铺Banner',
  `storeDesc` varchar(255) DEFAULT NULL COMMENT '店铺简介',
  `verifyStatus` int(11) NOT NULL COMMENT '审核状态',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `verifyTime` datetime DEFAULT NULL COMMENT '审核时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='分销商表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsdistributioncondition`
--

CREATE TABLE IF NOT EXISTS `corecmsdistributioncondition` (
  `id` int(11) NOT NULL COMMENT '序列',
  `gradeId` int(11) NOT NULL COMMENT '会员等级Id',
  `code` varchar(50) DEFAULT NULL COMMENT '升级条件编码',
  `parameters` varchar(255) DEFAULT NULL COMMENT '其它参数'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='分销商等级升级条件';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsdistributiongrade`
--

CREATE TABLE IF NOT EXISTS `corecmsdistributiongrade` (
  `id` int(11) NOT NULL COMMENT '等级序列',
  `name` varchar(50) DEFAULT NULL COMMENT '等级名称',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认等级',
  `isAutoUpGrade` bit(1) NOT NULL COMMENT '是否自动升级',
  `sortId` int(11) NOT NULL COMMENT '等级排序',
  `description` varchar(500) DEFAULT NULL COMMENT '等级说明'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='分销商等级设置表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsdistributionorder`
--

CREATE TABLE IF NOT EXISTS `corecmsdistributionorder` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户分销商id',
  `buyUserId` int(11) NOT NULL COMMENT '下单用户id',
  `orderId` varchar(50) DEFAULT NULL COMMENT '订单编号',
  `amount` decimal(10,0) NOT NULL COMMENT '结算金额',
  `isSettlement` int(11) NOT NULL COMMENT '是否结算',
  `level` int(11) NOT NULL COMMENT '层级',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='分销商订单记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsdistributionresult`
--

CREATE TABLE IF NOT EXISTS `corecmsdistributionresult` (
  `id` int(11) NOT NULL COMMENT '序列',
  `gradeId` int(11) NOT NULL COMMENT '会员等级Id',
  `code` varchar(50) DEFAULT NULL COMMENT '佣金编码',
  `parameters` longtext COMMENT '佣金设置序列化参数'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='等级佣金表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsform`
--

CREATE TABLE IF NOT EXISTS `corecmsform` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(100) DEFAULT NULL COMMENT '表单名称',
  `type` int(11) NOT NULL COMMENT '表单类型',
  `sort` int(11) NOT NULL COMMENT '表单排序',
  `images` longtext COMMENT '图集',
  `videoPath` varchar(255) DEFAULT NULL COMMENT '视频地址',
  `description` varchar(255) DEFAULT NULL COMMENT '表单描述',
  `headType` int(11) NOT NULL COMMENT '表头类型',
  `headTypeValue` varchar(200) DEFAULT NULL COMMENT '表单头值',
  `headTypeVideo` varchar(200) DEFAULT NULL COMMENT '表单视频',
  `buttonName` varchar(50) DEFAULT NULL COMMENT '表单提交按钮名称',
  `buttonColor` varchar(30) DEFAULT NULL COMMENT '表单按钮颜色',
  `isLogin` bit(1) NOT NULL COMMENT '是否需要登录',
  `times` int(11) NOT NULL COMMENT '可提交次数',
  `qrcode` varchar(200) DEFAULT NULL COMMENT '二维码图片地址',
  `returnMsg` varchar(200) DEFAULT NULL COMMENT '提交后提示语',
  `endDateTime` datetime NOT NULL COMMENT '结束时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='表单';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsformitem`
--

CREATE TABLE IF NOT EXISTS `corecmsformitem` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(50) DEFAULT NULL COMMENT '字段名称',
  `type` varchar(30) DEFAULT NULL COMMENT '字段类型',
  `validationType` varchar(30) DEFAULT NULL COMMENT '验证类型',
  `value` varchar(255) DEFAULT NULL COMMENT '表单值',
  `defaultValue` varchar(255) DEFAULT NULL COMMENT '默认值',
  `formId` int(11) NOT NULL COMMENT '表单id',
  `required` bit(1) NOT NULL COMMENT '是否必填',
  `sort` int(11) NOT NULL COMMENT '排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='表单项表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsformsubmit`
--

CREATE TABLE IF NOT EXISTS `corecmsformsubmit` (
  `id` int(11) NOT NULL COMMENT '序列',
  `formId` int(11) NOT NULL COMMENT '表单id',
  `formName` varchar(255) DEFAULT NULL COMMENT '表单名称',
  `userId` int(11) NOT NULL COMMENT '会员id',
  `money` decimal(10,0) NOT NULL COMMENT '总金额',
  `payStatus` bit(1) NOT NULL COMMENT '是否支付',
  `status` bit(1) NOT NULL COMMENT '是否处理',
  `feedback` varchar(255) DEFAULT NULL COMMENT '表单反馈',
  `ip` varchar(20) DEFAULT NULL COMMENT '提交人ip',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户对表的提交记录';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsformsubmitdetail`
--

CREATE TABLE IF NOT EXISTS `corecmsformsubmitdetail` (
  `id` int(11) NOT NULL COMMENT '序列',
  `submitId` int(11) NOT NULL COMMENT '提交表单id',
  `formId` int(11) NOT NULL COMMENT '表单id',
  `formItemId` int(11) NOT NULL COMMENT '表单项id',
  `formItemName` varchar(200) DEFAULT NULL COMMENT '表单项名称',
  `formItemValue` longtext COMMENT '表单项值'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='提交表单保存大文本值表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoods`
--

CREATE TABLE IF NOT EXISTS `corecmsgoods` (
  `id` int(11) NOT NULL COMMENT '商品ID',
  `bn` varchar(30) NOT NULL COMMENT '商品条码',
  `name` varchar(200) NOT NULL COMMENT '商品名称',
  `brief` varchar(255) DEFAULT NULL COMMENT '商品简介',
  `image` varchar(255) DEFAULT NULL COMMENT '缩略图',
  `images` longtext COMMENT '图集',
  `video` varchar(255) DEFAULT NULL COMMENT '视频',
  `productsDistributionType` int(11) NOT NULL COMMENT '佣金分配方式',
  `goodsCategoryId` int(11) NOT NULL COMMENT '商品分类',
  `goodsTypeId` int(11) NOT NULL COMMENT '商品类别',
  `brandId` int(11) NOT NULL COMMENT '品牌',
  `isNomalVirtual` bit(1) NOT NULL COMMENT '是否虚拟商品',
  `isMarketable` bit(1) NOT NULL COMMENT '是否上架',
  `unit` varchar(20) DEFAULT NULL COMMENT '商品单位',
  `intro` longtext COMMENT '商品详情',
  `spesDesc` longtext COMMENT '商品规格序列号存储',
  `parameters` longtext COMMENT '参数序列化',
  `commentsCount` int(11) NOT NULL COMMENT '评论次数',
  `viewCount` int(11) NOT NULL COMMENT '浏览次数',
  `buyCount` int(11) NOT NULL COMMENT '购买次数',
  `uptime` datetime DEFAULT NULL COMMENT '上架时间',
  `downtime` datetime DEFAULT NULL COMMENT '下架时间',
  `sort` int(11) NOT NULL COMMENT '商品排序',
  `labelIds` varchar(50) DEFAULT NULL COMMENT '标签id逗号分隔',
  `newSpec` longtext COMMENT '自定义规格名称',
  `openSpec` int(11) NOT NULL COMMENT '开启规则',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isRecommend` bit(1) NOT NULL COMMENT '是否推荐',
  `isHot` bit(1) NOT NULL COMMENT '是否热门',
  `isDel` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodsbrowsing`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodsbrowsing` (
  `id` int(11) NOT NULL COMMENT 'ID',
  `goodsId` int(11) NOT NULL COMMENT '商品id 关联goods.id',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `goodsName` varchar(200) NOT NULL COMMENT '商品名称',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `isdel` bit(1) NOT NULL COMMENT '删除标志'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品浏览记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodscategory`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodscategory` (
  `id` int(11) NOT NULL COMMENT '序列',
  `parentId` int(11) NOT NULL COMMENT '上级分类id',
  `name` varchar(20) NOT NULL COMMENT '分类名称',
  `typeId` int(11) NOT NULL COMMENT '类型ID 关联 goods_type.id',
  `sort` int(11) NOT NULL COMMENT '分类排序',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '分类图片ID',
  `isShow` bit(1) NOT NULL COMMENT '是否显示',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品分类';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodscategoryextend`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodscategoryextend` (
  `id` int(11) NOT NULL COMMENT '序列',
  `goodsId` int(11) DEFAULT NULL COMMENT '商品id',
  `goodsCategroyId` int(11) DEFAULT NULL COMMENT '商品分类id'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品分类扩展表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodscollection`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodscollection` (
  `id` int(11) NOT NULL COMMENT 'ID',
  `goodsId` int(11) NOT NULL COMMENT '商品id 关联goods.id',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `goodsName` varchar(200) NOT NULL COMMENT '商品名称',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品收藏表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodscomment`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodscomment` (
  `id` int(11) NOT NULL COMMENT '序列',
  `commentId` int(11) NOT NULL COMMENT '父级评价ID',
  `score` int(11) NOT NULL COMMENT '评价1-5星',
  `userId` int(11) NOT NULL COMMENT '评价用户ID',
  `goodsId` int(11) NOT NULL COMMENT '商品ID',
  `orderId` varchar(32) NOT NULL COMMENT '评价订单ID',
  `addon` longtext COMMENT '货品规格序列号存储',
  `images` longtext COMMENT '评价图片逗号分隔最多五张',
  `contentBody` longtext COMMENT '评价内容',
  `sellerContent` longtext COMMENT '商家回复',
  `isDisplay` bit(1) NOT NULL COMMENT '前台显示',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品评价表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodsgrade`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodsgrade` (
  `id` int(11) NOT NULL COMMENT '序列',
  `goodsId` int(11) NOT NULL COMMENT '商品id',
  `gradeId` int(11) NOT NULL COMMENT '会员等级id',
  `gradePrice` decimal(10,0) NOT NULL COMMENT '会员价'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品会员价表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodsimages`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodsimages` (
  `goodsId` int(11) NOT NULL COMMENT '商品ID',
  `imageId` varchar(50) NOT NULL COMMENT '图片ID',
  `sort` int(11) NOT NULL COMMENT '图片排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品图片关联表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodsparams`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodsparams` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(50) DEFAULT NULL COMMENT '参数名称',
  `value` longtext COMMENT '参数值',
  `type` varchar(10) DEFAULT NULL COMMENT '参数类型',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品参数表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodstype`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodstype` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(20) DEFAULT NULL COMMENT '类型名称',
  `parameters` longtext COMMENT '参数序列号数组'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品类型';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodstypeparams`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodstypeparams` (
  `paramsId` int(11) NOT NULL COMMENT '商品参数id',
  `typeId` int(11) NOT NULL COMMENT '商品类型id'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品参数类型关系表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodstypespec`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodstypespec` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '属性名称',
  `sort` int(11) NOT NULL COMMENT '属性排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品类型属性表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodstypespecrel`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodstypespecrel` (
  `specId` int(11) NOT NULL COMMENT '属性ID',
  `typeId` int(11) NOT NULL COMMENT '类型ID'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品类型和属性关联表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsgoodstypespecvalue`
--

CREATE TABLE IF NOT EXISTS `corecmsgoodstypespecvalue` (
  `id` int(11) NOT NULL COMMENT '序列',
  `specId` int(11) NOT NULL COMMENT '属性ID 关联goods_type_spec.id',
  `value` varchar(255) NOT NULL COMMENT '属性值',
  `sort` int(11) NOT NULL COMMENT '排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商品类型属性值表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsimages`
--

CREATE TABLE IF NOT EXISTS `corecmsimages` (
  `id` varchar(50) NOT NULL COMMENT '图片ID',
  `name` varchar(50) DEFAULT NULL COMMENT '图片名称',
  `url` varchar(255) DEFAULT NULL COMMENT '绝对地址',
  `path` varchar(255) DEFAULT NULL COMMENT '物理地址',
  `type` varchar(255) DEFAULT NULL COMMENT '存储引擎',
  `isDel` bit(1) DEFAULT NULL COMMENT '是否删除',
  `createTime` datetime DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='图片表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsinvoice`
--

CREATE TABLE IF NOT EXISTS `corecmsinvoice` (
  `id` int(11) NOT NULL COMMENT '序列',
  `category` int(11) NOT NULL COMMENT '开票类型',
  `sourceId` varchar(32) DEFAULT NULL COMMENT '资源ID',
  `userId` int(11) NOT NULL COMMENT '所属用户ID',
  `type` int(11) NOT NULL COMMENT '发票类型',
  `title` varchar(255) NOT NULL COMMENT '发票抬头',
  `taxNumber` varchar(32) NOT NULL COMMENT '发票税号',
  `amount` decimal(10,0) NOT NULL COMMENT '发票金额',
  `status` int(11) NOT NULL COMMENT '开票状态',
  `remarks` varchar(2000) DEFAULT NULL COMMENT '开票备注',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='发票表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsinvoicerecord`
--

CREATE TABLE IF NOT EXISTS `corecmsinvoicerecord` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(80) NOT NULL COMMENT '发票抬头',
  `code` varchar(30) NOT NULL COMMENT '发票税号',
  `frequency` int(11) NOT NULL COMMENT '被使用次数'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='发票信息记录';

-- --------------------------------------------------------

--
-- 表的结构 `corecmslabel`
--

CREATE TABLE IF NOT EXISTS `corecmslabel` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(20) DEFAULT NULL COMMENT '标签名称',
  `style` varchar(20) DEFAULT NULL COMMENT '标签样式'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='标签表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsloginlog`
--

CREATE TABLE IF NOT EXISTS `corecmsloginlog` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `state` int(11) DEFAULT NULL COMMENT '登录类型',
  `logTime` datetime DEFAULT NULL COMMENT '时间',
  `city` varchar(100) DEFAULT NULL COMMENT '地点城市',
  `ip` varchar(15) DEFAULT NULL COMMENT 'ip地址'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='登录日志';

-- --------------------------------------------------------

--
-- 表的结构 `corecmslogistics`
--

CREATE TABLE IF NOT EXISTS `corecmslogistics` (
  `id` int(11) NOT NULL COMMENT '序列',
  `logiName` varchar(50) NOT NULL COMMENT '物流公司名称',
  `logiCode` varchar(50) NOT NULL COMMENT '物流公司编码',
  `imgUrl` varchar(255) DEFAULT NULL COMMENT '物流logo',
  `phone` varchar(255) DEFAULT NULL COMMENT '物流电话',
  `url` varchar(255) DEFAULT NULL COMMENT '物流网址',
  `sort` int(11) NOT NULL COMMENT '排序',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='物流公司表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsmessage`
--

CREATE TABLE IF NOT EXISTS `corecmsmessage` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `code` varchar(50) NOT NULL COMMENT '消息编码',
  `parameters` longtext COMMENT '参数',
  `contentBody` longtext COMMENT '内容',
  `status` bit(1) NOT NULL COMMENT '是否查看',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='消息发送表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsmessagecenter`
--

CREATE TABLE IF NOT EXISTS `corecmsmessagecenter` (
  `id` int(11) NOT NULL COMMENT '序列',
  `code` varchar(50) NOT NULL COMMENT '编码',
  `description` varchar(255) DEFAULT NULL COMMENT '描述',
  `isSms` bit(1) NOT NULL COMMENT '启用短信',
  `isMessage` bit(1) NOT NULL COMMENT '启用站内消息',
  `isWxTempletMessage` bit(1) NOT NULL COMMENT '启用微信模板消息'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='消息配置表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsnotice`
--

CREATE TABLE IF NOT EXISTS `corecmsnotice` (
  `id` int(11) NOT NULL COMMENT '序列',
  `title` varchar(200) NOT NULL COMMENT '公告标题',
  `contentBody` longtext NOT NULL COMMENT '公告内容',
  `type` int(11) DEFAULT NULL COMMENT '公告类型',
  `sort` int(11) DEFAULT NULL COMMENT '排序',
  `isDel` bit(1) DEFAULT NULL COMMENT '软删除位  有时间代表已删除',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='公告表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsorder`
--

CREATE TABLE IF NOT EXISTS `corecmsorder` (
  `orderId` varchar(20) NOT NULL COMMENT '订单号',
  `goodsAmount` decimal(10,0) NOT NULL COMMENT '商品总价',
  `payedAmount` decimal(10,0) NOT NULL COMMENT '已支付的金额',
  `orderAmount` decimal(10,0) NOT NULL COMMENT '订单实际销售总额',
  `payStatus` int(11) NOT NULL COMMENT '支付状态',
  `shipStatus` int(11) NOT NULL COMMENT '发货状态',
  `status` int(11) NOT NULL COMMENT '订单状态',
  `orderType` int(11) NOT NULL COMMENT '订单类型',
  `receiptType` int(11) NOT NULL COMMENT '收货方式',
  `paymentCode` varchar(20) DEFAULT NULL COMMENT '支付方式代码',
  `paymentTime` datetime DEFAULT NULL COMMENT '支付时间',
  `logisticsId` int(11) NOT NULL COMMENT '配送方式ID 关联ship.id',
  `logisticsName` varchar(50) DEFAULT NULL COMMENT '配送方式名称',
  `costFreight` decimal(10,0) NOT NULL COMMENT '配送费用',
  `userId` int(11) NOT NULL COMMENT '用户ID 关联user.id',
  `sellerId` int(11) NOT NULL COMMENT '店铺ID 关联seller.id',
  `confirmStatus` int(11) NOT NULL COMMENT '售后状态',
  `confirmTime` datetime DEFAULT NULL COMMENT '确认收货时间',
  `storeId` int(11) NOT NULL COMMENT '自提门店ID，0就是不门店自提',
  `shipAreaId` int(11) NOT NULL COMMENT '收货地区ID',
  `shipAddress` varchar(200) DEFAULT NULL COMMENT '收货详细地址',
  `shipName` varchar(50) DEFAULT NULL COMMENT '收货人姓名',
  `shipMobile` varchar(50) DEFAULT NULL COMMENT '收货电话',
  `weight` decimal(10,0) NOT NULL COMMENT '商品总重量',
  `taxType` int(11) NOT NULL COMMENT '开发票',
  `taxCode` varchar(50) DEFAULT NULL COMMENT '税号',
  `taxTitle` varchar(50) DEFAULT NULL COMMENT '发票抬头',
  `point` int(11) NOT NULL COMMENT '使用积分',
  `pointMoney` decimal(10,0) NOT NULL COMMENT '积分抵扣金额',
  `orderDiscountAmount` decimal(10,0) NOT NULL COMMENT '订单优惠金额',
  `goodsDiscountAmount` decimal(10,0) NOT NULL COMMENT '商品优惠金额',
  `couponDiscountAmount` decimal(10,0) NOT NULL COMMENT '优惠券优惠额度',
  `coupon` longtext COMMENT '优惠券信息',
  `promotionList` varchar(255) DEFAULT NULL COMMENT '优惠信息',
  `memo` varchar(255) DEFAULT NULL COMMENT '买家备注',
  `ip` varchar(50) DEFAULT NULL COMMENT '下单IP',
  `mark` varchar(510) DEFAULT NULL COMMENT '卖家备注',
  `source` int(11) NOT NULL COMMENT '订单来源',
  `isComment` bit(1) NOT NULL COMMENT '是否评论',
  `isdel` bit(1) NOT NULL COMMENT '删除标志',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='订单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsorderitem`
--

CREATE TABLE IF NOT EXISTS `corecmsorderitem` (
  `id` int(11) NOT NULL COMMENT '序号',
  `orderId` varchar(20) NOT NULL COMMENT '订单ID 关联order.id',
  `goodsId` int(11) NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int(11) NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(30) DEFAULT NULL COMMENT '货品编码',
  `bn` varchar(30) DEFAULT NULL COMMENT '商品编码',
  `name` varchar(200) NOT NULL COMMENT '商品名称',
  `price` decimal(10,0) NOT NULL COMMENT '货品价格单价',
  `costprice` decimal(10,0) NOT NULL COMMENT '货品成本价单价',
  `mktprice` decimal(10,0) NOT NULL COMMENT '市场价',
  `imageUrl` varchar(100) NOT NULL COMMENT '图片',
  `nums` int(11) NOT NULL COMMENT '数量',
  `amount` decimal(10,0) NOT NULL COMMENT '总价',
  `promotionAmount` decimal(10,0) NOT NULL COMMENT '商品优惠总金额',
  `promotionList` varchar(255) DEFAULT NULL COMMENT '促销信息',
  `weight` decimal(10,0) NOT NULL COMMENT '总重量',
  `sendNums` int(11) NOT NULL COMMENT '发货数量',
  `addon` longtext COMMENT '货品明细序列号存储',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='订单明细表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsorderlog`
--

CREATE TABLE IF NOT EXISTS `corecmsorderlog` (
  `id` int(11) NOT NULL COMMENT 'ID',
  `orderId` varchar(20) DEFAULT NULL COMMENT '订单ID',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `type` int(11) NOT NULL COMMENT '类型',
  `msg` varchar(100) DEFAULT NULL COMMENT '描述介绍',
  `data` longtext COMMENT '请求的数据json',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='订单记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspages`
--

CREATE TABLE IF NOT EXISTS `corecmspages` (
  `id` int(11) NOT NULL,
  `code` varchar(50) DEFAULT NULL COMMENT '可视化区域编码',
  `name` varchar(50) DEFAULT NULL COMMENT '可编辑区域名称',
  `description` varchar(255) DEFAULT NULL COMMENT '描述',
  `layout` int(11) DEFAULT NULL COMMENT '布局样式编码，1，手机端',
  `type` int(11) DEFAULT NULL COMMENT '1手机端，2PC端'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='单页';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspagesitems`
--

CREATE TABLE IF NOT EXISTS `corecmspagesitems` (
  `id` int(11) NOT NULL,
  `widgetCode` varchar(50) NOT NULL COMMENT '组件编码',
  `pageCode` varchar(50) NOT NULL COMMENT '页面编码',
  `positionId` int(11) NOT NULL COMMENT '布局位置',
  `sort` int(11) NOT NULL COMMENT '排序，越小越靠前',
  `parameters` longtext COMMENT '组件配置内容'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='单页内容';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspayments`
--

CREATE TABLE IF NOT EXISTS `corecmspayments` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(50) DEFAULT NULL COMMENT '支付类型名称',
  `code` varchar(50) DEFAULT NULL COMMENT '支付类型编码',
  `isOnline` bit(1) NOT NULL COMMENT '是否线上支付',
  `parameters` longtext COMMENT '参数',
  `sort` int(11) NOT NULL COMMENT '排序',
  `memo` varchar(200) DEFAULT NULL COMMENT '方式描述',
  `isEnable` bit(1) NOT NULL COMMENT '是否启用'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='支付方式表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspaymentsfile`
--

CREATE TABLE IF NOT EXISTS `corecmspaymentsfile` (
  `id` int(11) NOT NULL COMMENT '视频ID',
  `name` varchar(50) DEFAULT NULL COMMENT '视频名称',
  `url` varchar(255) DEFAULT NULL COMMENT '绝对地址',
  `path` varchar(255) DEFAULT NULL COMMENT '物理地址',
  `type` varchar(255) DEFAULT NULL COMMENT '存储引擎',
  `fileType` varchar(255) DEFAULT NULL COMMENT '文件类型',
  `isDel` int(11) DEFAULT NULL COMMENT '是否删除',
  `createTime` datetime DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='文件表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspintuangoods`
--

CREATE TABLE IF NOT EXISTS `corecmspintuangoods` (
  `ruleId` int(11) NOT NULL COMMENT '规则表序列',
  `goodsId` int(11) NOT NULL COMMENT '商品序列'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='拼团商品表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspintuanrecord`
--

CREATE TABLE IF NOT EXISTS `corecmspintuanrecord` (
  `id` int(11) NOT NULL COMMENT '序列',
  `teamId` int(11) NOT NULL COMMENT '团序列',
  `userId` int(11) NOT NULL COMMENT '用户序列',
  `ruleId` int(11) NOT NULL COMMENT '规则表序列',
  `goodsId` int(11) NOT NULL COMMENT '商品序列',
  `status` int(11) NOT NULL COMMENT '状态',
  `orderId` varchar(20) NOT NULL COMMENT '订单序列',
  `parameters` varchar(200) DEFAULT NULL COMMENT '拼团人数Json',
  `closeTime` datetime NOT NULL COMMENT '关闭时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='拼团记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspintuanrule`
--

CREATE TABLE IF NOT EXISTS `corecmspintuanrule` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '活动名称',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间',
  `peopleNumber` int(11) NOT NULL COMMENT '人数2-10人',
  `significantInterval` int(11) NOT NULL COMMENT '单位分钟',
  `discountAmount` decimal(10,0) NOT NULL COMMENT '优惠金额',
  `maxNums` int(11) NOT NULL COMMENT '每人限购数量',
  `maxGoodsNums` int(11) NOT NULL COMMENT '每个商品活动数量',
  `sort` int(11) NOT NULL COMMENT '排序',
  `isStatusOpen` bit(1) NOT NULL COMMENT '是否开启',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='拼团规则表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsproducts`
--

CREATE TABLE IF NOT EXISTS `corecmsproducts` (
  `id` int(11) NOT NULL COMMENT '货品序列',
  `goodsId` int(11) NOT NULL COMMENT '商品序列',
  `barcode` varchar(128) DEFAULT NULL COMMENT '商品条码',
  `sn` varchar(30) DEFAULT NULL COMMENT '货品编码',
  `price` decimal(10,0) NOT NULL COMMENT '货品价格',
  `costprice` decimal(10,0) NOT NULL COMMENT '货品成本价',
  `mktprice` decimal(10,0) NOT NULL COMMENT '货品市场价',
  `marketable` bit(1) NOT NULL COMMENT '是否上架',
  `weight` decimal(10,0) NOT NULL COMMENT '重量(千克)',
  `stock` int(11) NOT NULL COMMENT '库存',
  `freezeStock` int(11) NOT NULL COMMENT '冻结库存',
  `spesDesc` longtext COMMENT '规格值',
  `isDefalut` bit(1) NOT NULL COMMENT '是否默认货品',
  `images` longtext COMMENT '规格图片',
  `isDel` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='货品表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsproductsdistribution`
--

CREATE TABLE IF NOT EXISTS `corecmsproductsdistribution` (
  `id` int(11) NOT NULL COMMENT '序号',
  `productsId` int(11) NOT NULL COMMENT '货品序列',
  `productsSN` varchar(50) NOT NULL COMMENT '货品货号',
  `levelOne` decimal(10,0) NOT NULL COMMENT '一级佣金',
  `levelTwo` decimal(10,0) NOT NULL COMMENT '二级佣金',
  `levelThree` decimal(10,0) NOT NULL COMMENT '三级佣金',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='货品三级佣金表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspromotion`
--

CREATE TABLE IF NOT EXISTS `corecmspromotion` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(40) NOT NULL COMMENT '促销名称',
  `type` int(11) NOT NULL COMMENT '类型',
  `sort` int(11) NOT NULL COMMENT '排序',
  `parameters` longtext COMMENT '其它参数',
  `maxNums` int(11) NOT NULL COMMENT '每人限购数量',
  `maxGoodsNums` int(11) NOT NULL COMMENT '每个商品活动数量',
  `maxRecevieNums` int(11) NOT NULL COMMENT '最大领取数量',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间',
  `isExclusive` bit(1) NOT NULL COMMENT '是否排他',
  `isAutoReceive` bit(1) NOT NULL COMMENT '是否自动领取',
  `isEnable` bit(1) NOT NULL COMMENT '是否开启',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  `effectiveDays` int(11) NOT NULL COMMENT '有效天数',
  `effectiveHours` int(11) NOT NULL COMMENT '有效小时'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='促销表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspromotioncondition`
--

CREATE TABLE IF NOT EXISTS `corecmspromotioncondition` (
  `id` int(11) NOT NULL COMMENT '序列',
  `promotionId` int(11) DEFAULT NULL COMMENT '促销ID',
  `code` varchar(50) DEFAULT NULL COMMENT '促销条件编码',
  `parameters` longtext COMMENT '支付配置参数序列号存储'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='促销条件表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspromotionrecord`
--

CREATE TABLE IF NOT EXISTS `corecmspromotionrecord` (
  `id` int(11) NOT NULL COMMENT '记录序列',
  `promotionId` int(11) NOT NULL COMMENT '促销序列',
  `userId` int(11) NOT NULL COMMENT '用户Id',
  `goodsId` int(11) NOT NULL COMMENT '商品id',
  `productId` int(11) NOT NULL COMMENT '货品id',
  `orderId` varchar(50) NOT NULL COMMENT '订单id',
  `type` int(11) NOT NULL COMMENT '3团购/4秒杀',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime NOT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='促销活动记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmspromotionresult`
--

CREATE TABLE IF NOT EXISTS `corecmspromotionresult` (
  `id` int(11) NOT NULL COMMENT '序列',
  `promotionId` int(11) DEFAULT NULL COMMENT '促销ID',
  `code` varchar(50) DEFAULT NULL COMMENT '促销条件编码',
  `parameters` longtext COMMENT '支付配置参数序列号存储'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='促销结果表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsservicedescription`
--

CREATE TABLE IF NOT EXISTS `corecmsservicedescription` (
  `id` int(11) NOT NULL COMMENT '序列',
  `title` varchar(100) NOT NULL COMMENT '名称',
  `type` int(11) NOT NULL COMMENT '类型',
  `description` varchar(500) NOT NULL COMMENT '描述',
  `isShow` bit(1) NOT NULL COMMENT '是否展示',
  `sortId` int(11) NOT NULL COMMENT '排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='商城服务说明';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsservices`
--

CREATE TABLE IF NOT EXISTS `corecmsservices` (
  `id` int(11) NOT NULL COMMENT '序列',
  `title` varchar(50) NOT NULL COMMENT '项目名称',
  `thumbnail` varchar(255) NOT NULL COMMENT '项目缩略图',
  `description` varchar(255) DEFAULT NULL COMMENT '项目概述',
  `contentBody` longtext NOT NULL COMMENT '项目详细说明',
  `allowedMembership` varchar(255) NOT NULL COMMENT '允许购买会员级别',
  `consumableStore` varchar(255) NOT NULL COMMENT '可消费门店',
  `status` int(11) NOT NULL COMMENT '项目状态',
  `maxBuyNumber` int(11) NOT NULL COMMENT '项目重复购买次数',
  `amount` int(11) NOT NULL COMMENT '项目可销售数量',
  `startTime` datetime NOT NULL COMMENT '项目开始时间',
  `endTime` datetime NOT NULL COMMENT '项目截止时间',
  `validityType` int(11) NOT NULL COMMENT '核销有效期类型',
  `validityStartTime` datetime DEFAULT NULL COMMENT '核销开始时间',
  `validityEndTime` datetime DEFAULT NULL COMMENT '核销结束时间',
  `ticketNumber` int(11) NOT NULL COMMENT '核销服务券数量',
  `createTime` datetime NOT NULL COMMENT '项目创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '项目更新时间',
  `money` decimal(10,0) NOT NULL COMMENT '售价'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='服务项目表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmssetting`
--

CREATE TABLE IF NOT EXISTS `corecmssetting` (
  `sKey` varchar(50) NOT NULL COMMENT '键',
  `sValue` longtext COMMENT '值'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='店铺设置表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsship`
--

CREATE TABLE IF NOT EXISTS `corecmsship` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(50) NOT NULL COMMENT '配送方式名称',
  `isCashOnDelivery` bit(1) NOT NULL COMMENT '是否货到付款',
  `firstUnit` int(11) NOT NULL COMMENT '首重',
  `continueUnit` int(11) NOT NULL COMMENT '续重',
  `isdefaultAreaFee` bit(1) NOT NULL COMMENT '是否按地区设置配送费用',
  `areaType` int(11) NOT NULL COMMENT '地区类型',
  `firstunitPrice` decimal(10,0) NOT NULL COMMENT '首重费用',
  `continueunitPrice` decimal(10,0) NOT NULL COMMENT '续重费用',
  `exp` longtext COMMENT '配送费用计算表达式',
  `logiName` varchar(50) DEFAULT NULL COMMENT '物流公司名称',
  `logiCode` varchar(50) DEFAULT NULL COMMENT '物流公司编码',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  `sort` int(11) NOT NULL COMMENT '配送方式排序',
  `status` int(11) NOT NULL COMMENT '状态1正常2停用',
  `isfreePostage` bit(1) NOT NULL COMMENT '是否包邮',
  `areaFee` longtext COMMENT '地区配送费用',
  `goodsMoney` decimal(10,0) NOT NULL COMMENT '商品总额满多少免运费'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='配送方式表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmssms`
--

CREATE TABLE IF NOT EXISTS `corecmssms` (
  `id` int(11) NOT NULL COMMENT '序列',
  `mobile` varchar(15) NOT NULL COMMENT '手机号码',
  `code` varchar(60) NOT NULL COMMENT '发送编码',
  `parameters` longtext NOT NULL COMMENT '参数',
  `contentBody` varchar(200) NOT NULL COMMENT '内容',
  `ip` varchar(50) NOT NULL COMMENT 'ip',
  `isUsed` bit(1) NOT NULL COMMENT '是否使用',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='短信发送日志';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsstock`
--

CREATE TABLE IF NOT EXISTS `corecmsstock` (
  `id` varchar(20) NOT NULL COMMENT '序列',
  `type` int(11) NOT NULL COMMENT '操作类型',
  `manager` int(11) NOT NULL COMMENT '操作员',
  `memo` varchar(200) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='库存操作表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsstocklog`
--

CREATE TABLE IF NOT EXISTS `corecmsstocklog` (
  `id` int(11) NOT NULL COMMENT '序列',
  `stockId` varchar(20) NOT NULL COMMENT '库存单号',
  `productId` int(11) NOT NULL COMMENT '货品序列',
  `goodsId` int(11) NOT NULL COMMENT '商品序列',
  `nums` int(11) NOT NULL COMMENT '数量',
  `sn` varchar(50) NOT NULL COMMENT '货品编码',
  `bn` varchar(50) NOT NULL COMMENT '商品条码',
  `goodsName` varchar(200) NOT NULL COMMENT '商品名称',
  `spesDesc` varchar(200) NOT NULL COMMENT '货品明细序列号存储'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='库存操作详情表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsstore`
--

CREATE TABLE IF NOT EXISTS `corecmsstore` (
  `id` int(11) NOT NULL COMMENT '序列',
  `storeName` varchar(125) DEFAULT NULL COMMENT '门店名称',
  `mobile` varchar(13) DEFAULT NULL COMMENT '门店电话/手机号',
  `linkMan` varchar(32) DEFAULT NULL COMMENT '门店联系人',
  `logoImage` varchar(255) DEFAULT NULL COMMENT '门店logo',
  `areaId` int(11) NOT NULL COMMENT '门店地区id',
  `address` varchar(200) DEFAULT NULL COMMENT '门店详细地址',
  `coordinate` varchar(50) DEFAULT NULL COMMENT '坐标位置',
  `latitude` varchar(40) DEFAULT NULL COMMENT '纬度',
  `longitude` varchar(40) DEFAULT NULL COMMENT '经度',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `distance` decimal(10,0) NOT NULL COMMENT '距离'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='门店表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuser`
--

CREATE TABLE IF NOT EXISTS `corecmsuser` (
  `id` int(11) NOT NULL COMMENT '用户ID',
  `userName` varchar(20) DEFAULT NULL COMMENT '用户名',
  `passWord` varchar(50) DEFAULT NULL COMMENT '密码',
  `mobile` varchar(15) DEFAULT NULL COMMENT '手机号',
  `sex` int(11) NOT NULL COMMENT '性别[1男2女3未知]',
  `birthday` datetime DEFAULT NULL COMMENT '生日',
  `avatarImage` varchar(255) DEFAULT NULL COMMENT '头像',
  `nickName` varchar(50) DEFAULT NULL COMMENT '昵称',
  `balance` decimal(10,0) NOT NULL COMMENT '余额',
  `point` int(11) NOT NULL COMMENT '积分',
  `grade` int(11) NOT NULL COMMENT '用户等级',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updataTime` datetime DEFAULT NULL COMMENT '更新时间',
  `status` int(11) NOT NULL COMMENT '状态[1正常2停用]',
  `parentId` int(11) NOT NULL COMMENT '推荐人',
  `userWx` int(11) NOT NULL COMMENT '关联三方账户',
  `isDelete` bit(1) NOT NULL COMMENT '删除标志 有数据就是删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserbalance`
--

CREATE TABLE IF NOT EXISTS `corecmsuserbalance` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `type` int(11) NOT NULL COMMENT '类型',
  `money` decimal(10,0) NOT NULL COMMENT '金额',
  `balance` decimal(10,0) NOT NULL COMMENT '余额',
  `sourceId` varchar(20) DEFAULT NULL COMMENT '资源id',
  `memo` varchar(200) DEFAULT NULL COMMENT '描述',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户余额表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserbankcard`
--

CREATE TABLE IF NOT EXISTS `corecmsuserbankcard` (
  `id` int(11) NOT NULL COMMENT 'id',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `bankName` varchar(60) DEFAULT NULL COMMENT '银行名称',
  `bankCode` varchar(12) DEFAULT NULL COMMENT '银行缩写',
  `bankAreaId` int(11) NOT NULL COMMENT '账号地区ID',
  `accountBank` varchar(255) DEFAULT NULL COMMENT '开户行',
  `accountName` varchar(60) DEFAULT NULL COMMENT '账户名',
  `cardNumber` varchar(30) DEFAULT NULL COMMENT '卡号',
  `cardType` int(11) NOT NULL COMMENT '银行卡类型',
  `isdefault` bit(1) NOT NULL COMMENT '默认卡',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '删除时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='银行卡信息';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsusergrade`
--

CREATE TABLE IF NOT EXISTS `corecmsusergrade` (
  `id` int(11) NOT NULL COMMENT 'id',
  `title` varchar(60) NOT NULL COMMENT '标题',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户等级表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserlog`
--

CREATE TABLE IF NOT EXISTS `corecmsuserlog` (
  `id` int(11) NOT NULL COMMENT 'id',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `state` int(11) DEFAULT NULL COMMENT '状态',
  `parameters` varchar(200) DEFAULT NULL COMMENT '参数',
  `ip` varchar(15) DEFAULT NULL COMMENT 'ip地址',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户日志';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserpointlog`
--

CREATE TABLE IF NOT EXISTS `corecmsuserpointlog` (
  `id` int(11) NOT NULL COMMENT 'ID',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `type` int(11) NOT NULL COMMENT '类型',
  `num` int(11) NOT NULL COMMENT '积分数量',
  `balance` int(11) NOT NULL COMMENT '积分余额',
  `remarks` varchar(255) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户积分记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserservicesorder`
--

CREATE TABLE IF NOT EXISTS `corecmsuserservicesorder` (
  `id` int(11) NOT NULL COMMENT '序列',
  `serviceOrderId` varchar(50) NOT NULL COMMENT '服务订单编号',
  `userId` int(11) NOT NULL COMMENT '关联用户',
  `servicesId` int(11) NOT NULL COMMENT '关联服务',
  `isPay` bit(1) NOT NULL COMMENT '是否支付',
  `payTime` datetime DEFAULT NULL COMMENT '支付时间',
  `paymentId` varchar(50) DEFAULT NULL COMMENT '支付单号',
  `status` int(11) NOT NULL COMMENT '状态',
  `createTime` datetime NOT NULL COMMENT '订单创建时间',
  `servicesEndTime` datetime DEFAULT NULL COMMENT '截止服务时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='服务购买表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserservicesticket`
--

CREATE TABLE IF NOT EXISTS `corecmsuserservicesticket` (
  `id` int(11) NOT NULL COMMENT '序列',
  `serviceOrderId` varchar(50) NOT NULL COMMENT '关联购买订单',
  `securityCode` varchar(36) NOT NULL COMMENT '安全码',
  `redeemCode` varchar(50) NOT NULL COMMENT '兑换码',
  `serviceId` int(11) NOT NULL COMMENT '关联服务项目id',
  `userId` int(11) NOT NULL COMMENT '关联用户id',
  `status` int(11) NOT NULL COMMENT '状态',
  `validityType` int(11) NOT NULL COMMENT '核销有效期类型',
  `validityStartTime` datetime DEFAULT NULL COMMENT '核销开始时间',
  `validityEndTime` datetime DEFAULT NULL COMMENT '核销结束时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `isVerification` bit(1) NOT NULL COMMENT '是否核销',
  `verificationTime` datetime DEFAULT NULL COMMENT '核销时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='服务消费券';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserservicesticketverificationlog`
--

CREATE TABLE IF NOT EXISTS `corecmsuserservicesticketverificationlog` (
  `id` int(11) NOT NULL COMMENT '序列',
  `storeId` int(11) NOT NULL COMMENT '核销门店id',
  `serviceId` int(11) NOT NULL COMMENT '关联服务',
  `verificationUserId` int(11) NOT NULL COMMENT '核验人',
  `ticketId` int(11) NOT NULL COMMENT '服务券序列',
  `ticketRedeemCode` varchar(50) NOT NULL COMMENT '核验码',
  `verificationTime` datetime NOT NULL COMMENT '核验时间',
  `isDel` bit(1) NOT NULL COMMENT '是否删除'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='服务券核验日志';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsusership`
--

CREATE TABLE IF NOT EXISTS `corecmsusership` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户id 关联user.id',
  `areaId` int(11) NOT NULL COMMENT '收货地区ID',
  `address` varchar(200) DEFAULT NULL COMMENT '收货详细地址',
  `name` varchar(50) DEFAULT NULL COMMENT '收货人姓名',
  `mobile` varchar(50) DEFAULT NULL COMMENT '收货电话',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户地址表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsusertocash`
--

CREATE TABLE IF NOT EXISTS `corecmsusertocash` (
  `id` int(11) NOT NULL COMMENT 'id',
  `userId` int(11) NOT NULL COMMENT '用户ID',
  `money` decimal(10,0) NOT NULL COMMENT '提现金额',
  `bankName` varchar(60) DEFAULT NULL COMMENT '银行名称',
  `bankCode` varchar(12) DEFAULT NULL COMMENT '银行缩写',
  `bankAreaId` int(11) DEFAULT NULL COMMENT '账号地区ID',
  `accountBank` varchar(255) DEFAULT NULL COMMENT '开户行',
  `accountName` varchar(60) DEFAULT NULL COMMENT '账户名',
  `cardNumber` varchar(30) DEFAULT NULL COMMENT '卡号',
  `withdrawals` decimal(10,0) NOT NULL COMMENT '提现服务费',
  `status` int(11) NOT NULL COMMENT '提现状态',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户提现记录表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsusertoken`
--

CREATE TABLE IF NOT EXISTS `corecmsusertoken` (
  `token` varchar(32) NOT NULL,
  `userId` int(11) NOT NULL COMMENT '用户序列',
  `platform` smallint(6) NOT NULL COMMENT '平台类型，1就是默认，2就是微信小程序',
  `createTime` datetime NOT NULL COMMENT '创建时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户token';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserwechatinfo`
--

CREATE TABLE IF NOT EXISTS `corecmsuserwechatinfo` (
  `id` int(11) NOT NULL COMMENT '用户ID',
  `type` int(11) DEFAULT NULL COMMENT '第三方登录类型',
  `userId` int(11) NOT NULL COMMENT '关联用户表',
  `openid` varchar(50) DEFAULT NULL COMMENT 'openId',
  `sessionKey` varchar(255) DEFAULT NULL COMMENT '缓存key',
  `unionId` varchar(50) DEFAULT NULL COMMENT 'unionid',
  `avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `nickName` varchar(50) DEFAULT NULL COMMENT '昵称',
  `gender` int(11) NOT NULL COMMENT '性别',
  `language` varchar(50) DEFAULT NULL COMMENT '语言',
  `city` varchar(80) DEFAULT NULL COMMENT '城市',
  `province` varchar(80) DEFAULT NULL COMMENT '省',
  `country` varchar(80) DEFAULT NULL COMMENT '国家',
  `countryCode` varchar(20) DEFAULT NULL COMMENT '手机号码国家编码',
  `mobile` varchar(20) DEFAULT NULL COMMENT '手机号码',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserwechatmsgsubscription`
--

CREATE TABLE IF NOT EXISTS `corecmsuserwechatmsgsubscription` (
  `id` int(11) NOT NULL COMMENT '序列',
  `templateId` varchar(50) NOT NULL COMMENT '模板Id',
  `userId` int(11) NOT NULL COMMENT '用户Id',
  `type` varchar(50) NOT NULL COMMENT '订阅类型'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='微信订阅消息存储表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserwechatmsgsubscriptionswitch`
--

CREATE TABLE IF NOT EXISTS `corecmsuserwechatmsgsubscriptionswitch` (
  `id` int(11) NOT NULL COMMENT '序列',
  `userId` int(11) NOT NULL COMMENT '用户Id',
  `isSwitch` bit(1) NOT NULL COMMENT '是否关闭'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户订阅提醒状态';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsuserwechatmsgtemplate`
--

CREATE TABLE IF NOT EXISTS `corecmsuserwechatmsgtemplate` (
  `id` int(11) NOT NULL COMMENT '序列',
  `templateTitle` varchar(255) DEFAULT NULL COMMENT '模板名称',
  `templateDes` varchar(255) DEFAULT NULL COMMENT '模板说明',
  `templateId` varchar(255) DEFAULT NULL COMMENT '模板Id',
  `data01` varchar(255) DEFAULT NULL COMMENT '字段1',
  `data02` varchar(255) DEFAULT NULL COMMENT '字段2',
  `data03` varchar(255) DEFAULT NULL COMMENT '字段3',
  `data04` varchar(255) DEFAULT NULL COMMENT '字段4',
  `data05` varchar(255) DEFAULT NULL COMMENT '字段5',
  `description` varchar(255) DEFAULT NULL COMMENT '描述',
  `sortId` int(11) NOT NULL COMMENT '排序'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='微信小程序消息模板';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsweixinauthor`
--

CREATE TABLE IF NOT EXISTS `corecmsweixinauthor` (
  `id` int(11) NOT NULL COMMENT '序列',
  `nickName` varchar(255) DEFAULT NULL COMMENT '授权方昵称',
  `headImg` varchar(255) DEFAULT NULL COMMENT '授权方头像',
  `serviceTypeInfo` varchar(10) DEFAULT NULL COMMENT '默认为0',
  `verifyTypeInfo` int(11) DEFAULT NULL COMMENT '授权方认证类型',
  `userName` varchar(200) DEFAULT NULL COMMENT '小程序的原始ID',
  `signature` longtext COMMENT '帐号介绍',
  `principalName` varchar(255) DEFAULT NULL COMMENT '小程序的主体名称',
  `businessInfo` longtext COMMENT '功能的开通状况（0代表未开通，1代表已开通）： open_store:是否开通微信门店功能 open_scan:是否开通微信扫商品功能 open_pay:是否开通微信支付功能 open_card:是否开通微信卡券功能 open_shake:是否开通微信摇一摇功能',
  `qrcodeUrl` varchar(255) DEFAULT NULL COMMENT '二维码图片的URL',
  `authorizationInfo` longtext COMMENT '授权信息',
  `appId` varchar(255) DEFAULT NULL COMMENT '授权方appid',
  `appSecret` varchar(100) DEFAULT NULL COMMENT '授权方AppSecret',
  `miniprograminfo` longtext COMMENT '可根据这个字段判断是否为小程序类型授权,有值为小程序',
  `funcInfo` longtext COMMENT '小程序授权给开发者的权限集列表，ID为17到19时分别代表： 17.帐号管理权限 18.开发管理权限 19.客服消息管理权限 请注意： 1）该字段的返回不会考虑小程序是否具备该权限集的权限（因为可能部分具备）',
  `authorizerRefreshToken` varchar(200) DEFAULT NULL COMMENT '刷新token',
  `authorizerAccessToken` varchar(200) DEFAULT NULL COMMENT 'token',
  `bindType` int(11) DEFAULT NULL COMMENT '绑定类型，1为第三方授权绑定，2为自助绑定',
  `authorType` varchar(10) DEFAULT NULL COMMENT '授权类型，默认b2c',
  `expiresIn` int(11) DEFAULT NULL COMMENT '绑定授权到期时间',
  `createTime` datetime DEFAULT NULL COMMENT '小程序授权时间',
  `updateTime` datetime DEFAULT NULL COMMENT '小程序更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='获取授权方的帐号基本信息表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsweixinmediamessage`
--

CREATE TABLE IF NOT EXISTS `corecmsweixinmediamessage` (
  `id` int(11) NOT NULL,
  `title` varchar(200) DEFAULT NULL COMMENT '标题',
  `author` varchar(100) DEFAULT NULL COMMENT '作者',
  `brief` varchar(255) DEFAULT NULL COMMENT '摘要',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '封面',
  `contentBody` longtext COMMENT '文章详情',
  `url` varchar(255) DEFAULT NULL COMMENT '原文地址',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='微信图文消息表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsweixinmenu`
--

CREATE TABLE IF NOT EXISTS `corecmsweixinmenu` (
  `id` int(11) NOT NULL,
  `menuId` int(11) NOT NULL COMMENT '菜单id',
  `parentId` int(11) NOT NULL COMMENT '父级菜单',
  `name` varchar(100) NOT NULL COMMENT '菜单名称',
  `type` varchar(11) NOT NULL COMMENT '菜单类型',
  `parameters` longtext NOT NULL COMMENT '菜单参数'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='微信公众号菜单表';

-- --------------------------------------------------------

--
-- 表的结构 `corecmsweixinmessage`
--

CREATE TABLE IF NOT EXISTS `corecmsweixinmessage` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(200) DEFAULT NULL COMMENT '消息名称',
  `type` int(11) DEFAULT NULL COMMENT '消息类型',
  `parameters` longtext COMMENT '消息参数',
  `isAttention` bit(1) DEFAULT NULL COMMENT '是否关注回复',
  `isDefault` bit(1) DEFAULT NULL COMMENT '是否默认回复',
  `isEnable` bit(1) DEFAULT NULL COMMENT '是否启用',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='微信消息表';

-- --------------------------------------------------------

--
-- 表的结构 `sysdictionary`
--

CREATE TABLE IF NOT EXISTS `sysdictionary` (
  `id` int(11) NOT NULL COMMENT '字典id',
  `dictCode` varchar(50) NOT NULL COMMENT '字典标识',
  `dictName` varchar(50) NOT NULL COMMENT '字典名称',
  `comments` varchar(500) DEFAULT NULL COMMENT '备注',
  `sortNumber` int(11) NOT NULL COMMENT '排序号',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='数据字典表';

-- --------------------------------------------------------

--
-- 表的结构 `sysdictionarydata`
--

CREATE TABLE IF NOT EXISTS `sysdictionarydata` (
  `id` int(11) NOT NULL COMMENT '字典项id',
  `dictId` int(11) DEFAULT NULL COMMENT '字典id',
  `dictDataCode` varchar(50) DEFAULT NULL COMMENT '字典项标识',
  `dictDataName` varchar(50) DEFAULT NULL COMMENT '字典项名称',
  `comments` varchar(50) DEFAULT NULL COMMENT '备注',
  `sortNumber` int(11) NOT NULL COMMENT '排序号',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='数据字典项表';

-- --------------------------------------------------------

--
-- 表的结构 `sysloginrecord`
--

CREATE TABLE IF NOT EXISTS `sysloginrecord` (
  `id` int(11) NOT NULL COMMENT '主键',
  `username` varchar(50) NOT NULL COMMENT '用户账号',
  `os` varchar(400) DEFAULT NULL COMMENT '操作系统',
  `device` varchar(100) DEFAULT NULL COMMENT '设备名',
  `browser` varchar(400) DEFAULT NULL COMMENT '浏览器类型',
  `ip` varchar(50) DEFAULT NULL COMMENT 'ip地址',
  `operType` int(11) NOT NULL COMMENT '操作类型',
  `comments` varchar(50) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '登录时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='登录日志表';

-- --------------------------------------------------------

--
-- 表的结构 `sysmenu`
--

CREATE TABLE IF NOT EXISTS `sysmenu` (
  `id` int(11) NOT NULL COMMENT '菜单id',
  `parentId` int(11) NOT NULL COMMENT '上级id,0是顶级',
  `identificationCode` varchar(100) DEFAULT NULL COMMENT '英文标识符',
  `menuName` varchar(100) DEFAULT NULL COMMENT '菜单名称',
  `menuIcon` varchar(100) DEFAULT NULL COMMENT '菜单图标',
  `path` varchar(100) DEFAULT NULL COMMENT '菜单路由关键字',
  `component` varchar(100) DEFAULT NULL COMMENT '菜单组件地址',
  `menuType` int(11) NOT NULL COMMENT '类型,0菜单,1按钮',
  `sortNumber` int(11) DEFAULT NULL COMMENT '排序号',
  `authority` varchar(100) DEFAULT NULL COMMENT '权限标识',
  `target` varchar(100) DEFAULT NULL COMMENT '打开位置',
  `iconColor` varchar(50) DEFAULT NULL COMMENT '菜单图标颜色',
  `hide` bit(1) NOT NULL COMMENT '是否隐藏,0否,1是',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='菜单表';

-- --------------------------------------------------------

--
-- 表的结构 `sysnlogrecords`
--

CREATE TABLE IF NOT EXISTS `sysnlogrecords` (
  `id` int(11) NOT NULL COMMENT '序列',
  `LogDate` datetime DEFAULT NULL COMMENT '时间',
  `LogLevel` varchar(50) DEFAULT NULL COMMENT '级别',
  `LogType` varchar(50) DEFAULT NULL COMMENT '事件日志上下文',
  `LogTitle` varchar(255) DEFAULT NULL COMMENT '事件标题',
  `Logger` varchar(255) DEFAULT NULL COMMENT '记录器名字',
  `Message` longtext COMMENT '消息',
  `Exception` longtext COMMENT '异常信息',
  `MachineName` varchar(50) DEFAULT NULL COMMENT '机器名称',
  `MachineIp` varchar(50) DEFAULT NULL COMMENT 'ip',
  `NetRequestMethod` varchar(50) DEFAULT NULL COMMENT '请求方式',
  `NetRequestUrl` varchar(500) DEFAULT NULL COMMENT '请求地址',
  `NetUserIsauthenticated` varchar(50) DEFAULT NULL COMMENT '是否授权',
  `NetUserAuthtype` varchar(50) DEFAULT NULL COMMENT '授权类型',
  `NetUserIdentity` varchar(50) DEFAULT NULL COMMENT '身份认证'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='Nlog记录表';

-- --------------------------------------------------------

--
-- 表的结构 `sysoperrecord`
--

CREATE TABLE IF NOT EXISTS `sysoperrecord` (
  `id` int(11) NOT NULL COMMENT '主键',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `userName` varchar(50) DEFAULT NULL COMMENT '用户名',
  `model` varchar(50) DEFAULT NULL COMMENT '操作模块',
  `description` varchar(50) DEFAULT NULL COMMENT '操作方法',
  `url` varchar(255) DEFAULT NULL COMMENT '请求地址',
  `requestMethod` varchar(50) DEFAULT NULL COMMENT '请求方式',
  `operMethod` varchar(50) DEFAULT NULL COMMENT '调用方法',
  `param` longtext COMMENT '请求参数',
  `result` longtext COMMENT '返回结果',
  `ip` varchar(50) DEFAULT NULL COMMENT 'ip地址',
  `spendTime` varchar(50) NOT NULL COMMENT '请求耗时,单位毫秒',
  `state` int(11) NOT NULL COMMENT '状态,0成功,1异常',
  `createTime` datetime NOT NULL COMMENT '登录时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='操作日志表';

-- --------------------------------------------------------

--
-- 表的结构 `sysorganization`
--

CREATE TABLE IF NOT EXISTS `sysorganization` (
  `id` int(11) NOT NULL COMMENT '机构id',
  `parentId` int(11) NOT NULL COMMENT '上级id,0是顶级',
  `organizationName` varchar(50) DEFAULT NULL COMMENT '机构名称',
  `organizationFullName` varchar(50) DEFAULT NULL COMMENT '机构名称',
  `organizationType` int(11) NOT NULL COMMENT '机构类型',
  `leaderId` int(11) DEFAULT NULL COMMENT '负责人id',
  `sortNumber` int(11) NOT NULL COMMENT '排序号',
  `comments` varchar(500) DEFAULT NULL COMMENT '备注',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='组织机构表';

-- --------------------------------------------------------

--
-- 表的结构 `sysrole`
--

CREATE TABLE IF NOT EXISTS `sysrole` (
  `id` int(11) NOT NULL COMMENT '角色id',
  `roleName` varchar(50) NOT NULL COMMENT '角色名称',
  `roleCode` varchar(50) DEFAULT NULL COMMENT '角色标识',
  `comments` varchar(255) DEFAULT NULL COMMENT '备注',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='角色表';

-- --------------------------------------------------------

--
-- 表的结构 `sysrolemenu`
--

CREATE TABLE IF NOT EXISTS `sysrolemenu` (
  `id` int(11) NOT NULL COMMENT '主键',
  `roleId` int(11) NOT NULL COMMENT '角色id',
  `menuId` int(11) NOT NULL COMMENT '菜单id',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='角色菜单关联表';

-- --------------------------------------------------------

--
-- 表的结构 `systasklog`
--

CREATE TABLE IF NOT EXISTS `systasklog` (
  `id` int(11) NOT NULL COMMENT '序列',
  `name` varchar(50) NOT NULL COMMENT '任务名称',
  `createTime` datetime NOT NULL COMMENT '完成时间',
  `isSuccess` bit(1) NOT NULL COMMENT '是否完成',
  `parameters` longtext COMMENT '其他数据'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='定时任务日志';

-- --------------------------------------------------------

--
-- 表的结构 `sysuser`
--

CREATE TABLE IF NOT EXISTS `sysuser` (
  `id` int(11) NOT NULL COMMENT '用户id',
  `userName` varchar(50) DEFAULT NULL COMMENT '账号',
  `passWord` varchar(100) DEFAULT NULL COMMENT '密码',
  `nickName` varchar(50) DEFAULT NULL COMMENT '昵称',
  `avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `sex` int(11) NOT NULL COMMENT '性别',
  `phone` varchar(50) DEFAULT NULL COMMENT '手机号',
  `email` varchar(50) DEFAULT NULL COMMENT '邮箱',
  `emailVerified` bit(1) NOT NULL COMMENT '邮箱是否验证',
  `trueName` varchar(50) DEFAULT NULL COMMENT '真实姓名',
  `idCard` varchar(50) DEFAULT NULL COMMENT '身份证号',
  `birthday` datetime DEFAULT NULL COMMENT '出生日期',
  `introduction` varchar(500) DEFAULT NULL COMMENT '个人简介',
  `organizationId` int(11) DEFAULT NULL COMMENT '机构id',
  `state` int(11) NOT NULL COMMENT '状态,0正常,1冻结',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '注册时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户表';

-- --------------------------------------------------------

--
-- 表的结构 `sysuserrole`
--

CREATE TABLE IF NOT EXISTS `sysuserrole` (
  `id` int(11) NOT NULL COMMENT '主键',
  `userId` int(11) NOT NULL COMMENT '用户id',
  `roleId` int(11) NOT NULL COMMENT '角色id',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间'
) ENGINE=MyISAM DEFAULT CHARSET=utf8 COMMENT='用户角色关联表';

--
-- Indexes for dumped tables
--

--
-- Indexes for table `corecmsadvertisement`
--
ALTER TABLE `corecmsadvertisement`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsadvertposition`
--
ALTER TABLE `corecmsadvertposition`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsagent`
--
ALTER TABLE `corecmsagent`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsagentgoods`
--
ALTER TABLE `corecmsagentgoods`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsagentgrade`
--
ALTER TABLE `corecmsagentgrade`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsagentorder`
--
ALTER TABLE `corecmsagentorder`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsagentproducts`
--
ALTER TABLE `corecmsagentproducts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsapiaccesstoken`
--
ALTER TABLE `corecmsapiaccesstoken`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsarea`
--
ALTER TABLE `corecmsarea`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsarticle`
--
ALTER TABLE `corecmsarticle`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsarticletype`
--
ALTER TABLE `corecmsarticletype`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsbillaftersales`
--
ALTER TABLE `corecmsbillaftersales`
  ADD PRIMARY KEY (`aftersalesId`);

--
-- Indexes for table `corecmsbillaftersalesitem`
--
ALTER TABLE `corecmsbillaftersalesitem`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsbilldelivery`
--
ALTER TABLE `corecmsbilldelivery`
  ADD PRIMARY KEY (`deliveryId`);

--
-- Indexes for table `corecmsbilldeliveryitem`
--
ALTER TABLE `corecmsbilldeliveryitem`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsbilldeliveryorderrel`
--
ALTER TABLE `corecmsbilldeliveryorderrel`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsbilllading`
--
ALTER TABLE `corecmsbilllading`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsbillpayments`
--
ALTER TABLE `corecmsbillpayments`
  ADD PRIMARY KEY (`paymentId`);

--
-- Indexes for table `corecmsbillrefund`
--
ALTER TABLE `corecmsbillrefund`
  ADD PRIMARY KEY (`refundId`);

--
-- Indexes for table `corecmsbillreship`
--
ALTER TABLE `corecmsbillreship`
  ADD PRIMARY KEY (`reshipId`);

--
-- Indexes for table `corecmsbillreshipitem`
--
ALTER TABLE `corecmsbillreshipitem`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsbrand`
--
ALTER TABLE `corecmsbrand`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmscart`
--
ALTER TABLE `corecmscart`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsclerk`
--
ALTER TABLE `corecmsclerk`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmscoupon`
--
ALTER TABLE `corecmscoupon`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsdistribution`
--
ALTER TABLE `corecmsdistribution`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsdistributioncondition`
--
ALTER TABLE `corecmsdistributioncondition`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsdistributiongrade`
--
ALTER TABLE `corecmsdistributiongrade`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsdistributionorder`
--
ALTER TABLE `corecmsdistributionorder`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsdistributionresult`
--
ALTER TABLE `corecmsdistributionresult`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsform`
--
ALTER TABLE `corecmsform`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsformitem`
--
ALTER TABLE `corecmsformitem`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsformsubmit`
--
ALTER TABLE `corecmsformsubmit`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsformsubmitdetail`
--
ALTER TABLE `corecmsformsubmitdetail`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoods`
--
ALTER TABLE `corecmsgoods`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodsbrowsing`
--
ALTER TABLE `corecmsgoodsbrowsing`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodscategory`
--
ALTER TABLE `corecmsgoodscategory`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodscategoryextend`
--
ALTER TABLE `corecmsgoodscategoryextend`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodscollection`
--
ALTER TABLE `corecmsgoodscollection`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodscomment`
--
ALTER TABLE `corecmsgoodscomment`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodsgrade`
--
ALTER TABLE `corecmsgoodsgrade`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodsparams`
--
ALTER TABLE `corecmsgoodsparams`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodstype`
--
ALTER TABLE `corecmsgoodstype`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodstypespec`
--
ALTER TABLE `corecmsgoodstypespec`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsgoodstypespecvalue`
--
ALTER TABLE `corecmsgoodstypespecvalue`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsimages`
--
ALTER TABLE `corecmsimages`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsinvoice`
--
ALTER TABLE `corecmsinvoice`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsinvoicerecord`
--
ALTER TABLE `corecmsinvoicerecord`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmslabel`
--
ALTER TABLE `corecmslabel`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsloginlog`
--
ALTER TABLE `corecmsloginlog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmslogistics`
--
ALTER TABLE `corecmslogistics`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsmessage`
--
ALTER TABLE `corecmsmessage`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsmessagecenter`
--
ALTER TABLE `corecmsmessagecenter`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsnotice`
--
ALTER TABLE `corecmsnotice`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsorder`
--
ALTER TABLE `corecmsorder`
  ADD PRIMARY KEY (`orderId`);

--
-- Indexes for table `corecmsorderitem`
--
ALTER TABLE `corecmsorderitem`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsorderlog`
--
ALTER TABLE `corecmsorderlog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspages`
--
ALTER TABLE `corecmspages`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspagesitems`
--
ALTER TABLE `corecmspagesitems`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspayments`
--
ALTER TABLE `corecmspayments`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspaymentsfile`
--
ALTER TABLE `corecmspaymentsfile`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspintuanrecord`
--
ALTER TABLE `corecmspintuanrecord`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspintuanrule`
--
ALTER TABLE `corecmspintuanrule`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsproducts`
--
ALTER TABLE `corecmsproducts`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsproductsdistribution`
--
ALTER TABLE `corecmsproductsdistribution`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspromotion`
--
ALTER TABLE `corecmspromotion`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspromotioncondition`
--
ALTER TABLE `corecmspromotioncondition`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspromotionrecord`
--
ALTER TABLE `corecmspromotionrecord`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmspromotionresult`
--
ALTER TABLE `corecmspromotionresult`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsservicedescription`
--
ALTER TABLE `corecmsservicedescription`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsservices`
--
ALTER TABLE `corecmsservices`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmssetting`
--
ALTER TABLE `corecmssetting`
  ADD PRIMARY KEY (`sKey`);

--
-- Indexes for table `corecmsship`
--
ALTER TABLE `corecmsship`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmssms`
--
ALTER TABLE `corecmssms`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsstock`
--
ALTER TABLE `corecmsstock`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsstocklog`
--
ALTER TABLE `corecmsstocklog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsstore`
--
ALTER TABLE `corecmsstore`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuser`
--
ALTER TABLE `corecmsuser`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserbalance`
--
ALTER TABLE `corecmsuserbalance`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserbankcard`
--
ALTER TABLE `corecmsuserbankcard`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsusergrade`
--
ALTER TABLE `corecmsusergrade`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserlog`
--
ALTER TABLE `corecmsuserlog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserpointlog`
--
ALTER TABLE `corecmsuserpointlog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserservicesorder`
--
ALTER TABLE `corecmsuserservicesorder`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserservicesticket`
--
ALTER TABLE `corecmsuserservicesticket`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserservicesticketverificationlog`
--
ALTER TABLE `corecmsuserservicesticketverificationlog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsusership`
--
ALTER TABLE `corecmsusership`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsusertocash`
--
ALTER TABLE `corecmsusertocash`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsusertoken`
--
ALTER TABLE `corecmsusertoken`
  ADD PRIMARY KEY (`token`);

--
-- Indexes for table `corecmsuserwechatinfo`
--
ALTER TABLE `corecmsuserwechatinfo`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserwechatmsgsubscription`
--
ALTER TABLE `corecmsuserwechatmsgsubscription`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserwechatmsgsubscriptionswitch`
--
ALTER TABLE `corecmsuserwechatmsgsubscriptionswitch`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsuserwechatmsgtemplate`
--
ALTER TABLE `corecmsuserwechatmsgtemplate`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsweixinauthor`
--
ALTER TABLE `corecmsweixinauthor`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsweixinmediamessage`
--
ALTER TABLE `corecmsweixinmediamessage`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsweixinmenu`
--
ALTER TABLE `corecmsweixinmenu`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `corecmsweixinmessage`
--
ALTER TABLE `corecmsweixinmessage`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysdictionary`
--
ALTER TABLE `sysdictionary`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysdictionarydata`
--
ALTER TABLE `sysdictionarydata`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysloginrecord`
--
ALTER TABLE `sysloginrecord`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysmenu`
--
ALTER TABLE `sysmenu`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysnlogrecords`
--
ALTER TABLE `sysnlogrecords`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysoperrecord`
--
ALTER TABLE `sysoperrecord`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysorganization`
--
ALTER TABLE `sysorganization`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysrole`
--
ALTER TABLE `sysrole`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysrolemenu`
--
ALTER TABLE `sysrolemenu`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `systasklog`
--
ALTER TABLE `systasklog`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysuser`
--
ALTER TABLE `sysuser`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `sysuserrole`
--
ALTER TABLE `sysuserrole`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `corecmsadvertisement`
--
ALTER TABLE `corecmsadvertisement`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsadvertposition`
--
ALTER TABLE `corecmsadvertposition`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsagent`
--
ALTER TABLE `corecmsagent`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsagentgoods`
--
ALTER TABLE `corecmsagentgoods`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsagentgrade`
--
ALTER TABLE `corecmsagentgrade`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '等级序列';
--
-- AUTO_INCREMENT for table `corecmsagentorder`
--
ALTER TABLE `corecmsagentorder`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsagentproducts`
--
ALTER TABLE `corecmsagentproducts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsapiaccesstoken`
--
ALTER TABLE `corecmsapiaccesstoken`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsarea`
--
ALTER TABLE `corecmsarea`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '地区ID';
--
-- AUTO_INCREMENT for table `corecmsarticle`
--
ALTER TABLE `corecmsarticle`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsarticletype`
--
ALTER TABLE `corecmsarticletype`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsbillaftersalesitem`
--
ALTER TABLE `corecmsbillaftersalesitem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsbilldeliveryitem`
--
ALTER TABLE `corecmsbilldeliveryitem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsbilldeliveryorderrel`
--
ALTER TABLE `corecmsbilldeliveryorderrel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID';
--
-- AUTO_INCREMENT for table `corecmsbillreshipitem`
--
ALTER TABLE `corecmsbillreshipitem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsbrand`
--
ALTER TABLE `corecmsbrand`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '品牌ID';
--
-- AUTO_INCREMENT for table `corecmscart`
--
ALTER TABLE `corecmscart`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsclerk`
--
ALTER TABLE `corecmsclerk`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmscoupon`
--
ALTER TABLE `corecmscoupon`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsdistribution`
--
ALTER TABLE `corecmsdistribution`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsdistributioncondition`
--
ALTER TABLE `corecmsdistributioncondition`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsdistributiongrade`
--
ALTER TABLE `corecmsdistributiongrade`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '等级序列';
--
-- AUTO_INCREMENT for table `corecmsdistributionorder`
--
ALTER TABLE `corecmsdistributionorder`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsdistributionresult`
--
ALTER TABLE `corecmsdistributionresult`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsform`
--
ALTER TABLE `corecmsform`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsformitem`
--
ALTER TABLE `corecmsformitem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsformsubmit`
--
ALTER TABLE `corecmsformsubmit`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsformsubmitdetail`
--
ALTER TABLE `corecmsformsubmitdetail`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoods`
--
ALTER TABLE `corecmsgoods`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '商品ID';
--
-- AUTO_INCREMENT for table `corecmsgoodsbrowsing`
--
ALTER TABLE `corecmsgoodsbrowsing`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID';
--
-- AUTO_INCREMENT for table `corecmsgoodscategory`
--
ALTER TABLE `corecmsgoodscategory`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodscategoryextend`
--
ALTER TABLE `corecmsgoodscategoryextend`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodscollection`
--
ALTER TABLE `corecmsgoodscollection`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID';
--
-- AUTO_INCREMENT for table `corecmsgoodscomment`
--
ALTER TABLE `corecmsgoodscomment`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodsgrade`
--
ALTER TABLE `corecmsgoodsgrade`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodsparams`
--
ALTER TABLE `corecmsgoodsparams`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodstype`
--
ALTER TABLE `corecmsgoodstype`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodstypespec`
--
ALTER TABLE `corecmsgoodstypespec`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsgoodstypespecvalue`
--
ALTER TABLE `corecmsgoodstypespecvalue`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsinvoice`
--
ALTER TABLE `corecmsinvoice`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsinvoicerecord`
--
ALTER TABLE `corecmsinvoicerecord`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmslabel`
--
ALTER TABLE `corecmslabel`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsloginlog`
--
ALTER TABLE `corecmsloginlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmslogistics`
--
ALTER TABLE `corecmslogistics`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsmessage`
--
ALTER TABLE `corecmsmessage`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsmessagecenter`
--
ALTER TABLE `corecmsmessagecenter`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsnotice`
--
ALTER TABLE `corecmsnotice`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsorderitem`
--
ALTER TABLE `corecmsorderitem`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序号';
--
-- AUTO_INCREMENT for table `corecmsorderlog`
--
ALTER TABLE `corecmsorderlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID';
--
-- AUTO_INCREMENT for table `corecmspages`
--
ALTER TABLE `corecmspages`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `corecmspagesitems`
--
ALTER TABLE `corecmspagesitems`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `corecmspayments`
--
ALTER TABLE `corecmspayments`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmspaymentsfile`
--
ALTER TABLE `corecmspaymentsfile`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '视频ID';
--
-- AUTO_INCREMENT for table `corecmspintuanrecord`
--
ALTER TABLE `corecmspintuanrecord`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmspintuanrule`
--
ALTER TABLE `corecmspintuanrule`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsproducts`
--
ALTER TABLE `corecmsproducts`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '货品序列';
--
-- AUTO_INCREMENT for table `corecmsproductsdistribution`
--
ALTER TABLE `corecmsproductsdistribution`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序号';
--
-- AUTO_INCREMENT for table `corecmspromotion`
--
ALTER TABLE `corecmspromotion`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmspromotioncondition`
--
ALTER TABLE `corecmspromotioncondition`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmspromotionrecord`
--
ALTER TABLE `corecmspromotionrecord`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '记录序列';
--
-- AUTO_INCREMENT for table `corecmspromotionresult`
--
ALTER TABLE `corecmspromotionresult`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsservicedescription`
--
ALTER TABLE `corecmsservicedescription`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsservices`
--
ALTER TABLE `corecmsservices`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsship`
--
ALTER TABLE `corecmsship`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmssms`
--
ALTER TABLE `corecmssms`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsstocklog`
--
ALTER TABLE `corecmsstocklog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsstore`
--
ALTER TABLE `corecmsstore`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsuser`
--
ALTER TABLE `corecmsuser`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户ID';
--
-- AUTO_INCREMENT for table `corecmsuserbalance`
--
ALTER TABLE `corecmsuserbalance`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsuserbankcard`
--
ALTER TABLE `corecmsuserbankcard`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id';
--
-- AUTO_INCREMENT for table `corecmsusergrade`
--
ALTER TABLE `corecmsusergrade`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id';
--
-- AUTO_INCREMENT for table `corecmsuserlog`
--
ALTER TABLE `corecmsuserlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id';
--
-- AUTO_INCREMENT for table `corecmsuserpointlog`
--
ALTER TABLE `corecmsuserpointlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'ID';
--
-- AUTO_INCREMENT for table `corecmsuserservicesorder`
--
ALTER TABLE `corecmsuserservicesorder`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsuserservicesticket`
--
ALTER TABLE `corecmsuserservicesticket`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsuserservicesticketverificationlog`
--
ALTER TABLE `corecmsuserservicesticketverificationlog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsusership`
--
ALTER TABLE `corecmsusership`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsusertocash`
--
ALTER TABLE `corecmsusertocash`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'id';
--
-- AUTO_INCREMENT for table `corecmsuserwechatinfo`
--
ALTER TABLE `corecmsuserwechatinfo`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户ID';
--
-- AUTO_INCREMENT for table `corecmsuserwechatmsgsubscription`
--
ALTER TABLE `corecmsuserwechatmsgsubscription`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsuserwechatmsgsubscriptionswitch`
--
ALTER TABLE `corecmsuserwechatmsgsubscriptionswitch`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsuserwechatmsgtemplate`
--
ALTER TABLE `corecmsuserwechatmsgtemplate`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsweixinauthor`
--
ALTER TABLE `corecmsweixinauthor`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `corecmsweixinmediamessage`
--
ALTER TABLE `corecmsweixinmediamessage`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `corecmsweixinmenu`
--
ALTER TABLE `corecmsweixinmenu`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `corecmsweixinmessage`
--
ALTER TABLE `corecmsweixinmessage`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `sysdictionary`
--
ALTER TABLE `sysdictionary`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '字典id';
--
-- AUTO_INCREMENT for table `sysdictionarydata`
--
ALTER TABLE `sysdictionarydata`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '字典项id';
--
-- AUTO_INCREMENT for table `sysloginrecord`
--
ALTER TABLE `sysloginrecord`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键';
--
-- AUTO_INCREMENT for table `sysmenu`
--
ALTER TABLE `sysmenu`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '菜单id';
--
-- AUTO_INCREMENT for table `sysnlogrecords`
--
ALTER TABLE `sysnlogrecords`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `sysoperrecord`
--
ALTER TABLE `sysoperrecord`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键';
--
-- AUTO_INCREMENT for table `sysorganization`
--
ALTER TABLE `sysorganization`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '机构id';
--
-- AUTO_INCREMENT for table `sysrole`
--
ALTER TABLE `sysrole`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '角色id';
--
-- AUTO_INCREMENT for table `sysrolemenu`
--
ALTER TABLE `sysrolemenu`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键';
--
-- AUTO_INCREMENT for table `systasklog`
--
ALTER TABLE `systasklog`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '序列';
--
-- AUTO_INCREMENT for table `sysuser`
--
ALTER TABLE `sysuser`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '用户id';
--
-- AUTO_INCREMENT for table `sysuserrole`
--
ALTER TABLE `sysuserrole`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT COMMENT '主键';
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
