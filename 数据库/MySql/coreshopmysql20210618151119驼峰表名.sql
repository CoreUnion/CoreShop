/*
MySQL Backup
Database: shop
Backup Time: 2021-06-18 15:11:33
*/

SET FOREIGN_KEY_CHECKS=0;
DROP TABLE IF EXISTS `shop`.`CoreCmsAdvertPosition`;
DROP TABLE IF EXISTS `shop`.`CoreCmsAdvertisement`;
DROP TABLE IF EXISTS `shop`.`CoreCmsAgentGoods`;
DROP TABLE IF EXISTS `shop`.`CoreCmsAgentGrade`;
DROP TABLE IF EXISTS `shop`.`CoreCmsAgentOrder`;
DROP TABLE IF EXISTS `shop`.`CoreCmsAgentProducts`;
DROP TABLE IF EXISTS `shop`.`CoreCmsAgent`;
DROP TABLE IF EXISTS `shop`.`CoreCmsApiAccessToken`;
DROP TABLE IF EXISTS `shop`.`CoreCmsArea`;
DROP TABLE IF EXISTS `shop`.`CoreCmsArticleType`;
DROP TABLE IF EXISTS `shop`.`CoreCmsArticle`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillAftersalesImages`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillAftersalesItem`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillAftersales`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillDeliveryItem`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillDeliveryOrderRel`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillDelivery`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillLading`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillPaymentsRel`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillPayments`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillRefund`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillReshipItem`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBillReship`;
DROP TABLE IF EXISTS `shop`.`CoreCmsBrand`;
DROP TABLE IF EXISTS `shop`.`CoreCmsCart`;
DROP TABLE IF EXISTS `shop`.`CoreCmsClerk`;
DROP TABLE IF EXISTS `shop`.`CoreCmsCoupon`;
DROP TABLE IF EXISTS `shop`.`CoreCmsDistributionCondition`;
DROP TABLE IF EXISTS `shop`.`CoreCmsDistributionGrade`;
DROP TABLE IF EXISTS `shop`.`CoreCmsDistributionOrder`;
DROP TABLE IF EXISTS `shop`.`CoreCmsDistributionResult`;
DROP TABLE IF EXISTS `shop`.`CoreCmsDistribution`;
DROP TABLE IF EXISTS `shop`.`CoreCmsFormItem`;
DROP TABLE IF EXISTS `shop`.`CoreCmsFormSubmitDetail`;
DROP TABLE IF EXISTS `shop`.`CoreCmsFormSubmit`;
DROP TABLE IF EXISTS `shop`.`CoreCmsForm`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsBrowsing`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsCategoryExtend`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsCategory`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsCollection`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsComment`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsGrade`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsImages`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsParams`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsTypeParams`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsTypeSpecRel`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsTypeSpecValue`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsTypeSpec`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoodsType`;
DROP TABLE IF EXISTS `shop`.`CoreCmsGoods`;
DROP TABLE IF EXISTS `shop`.`CoreCmsImages`;
DROP TABLE IF EXISTS `shop`.`CoreCmsInvoiceRecord`;
DROP TABLE IF EXISTS `shop`.`CoreCmsInvoice`;
DROP TABLE IF EXISTS `shop`.`CoreCmsLabel`;
DROP TABLE IF EXISTS `shop`.`CoreCmsLoginLog`;
DROP TABLE IF EXISTS `shop`.`CoreCmsLogistics`;
DROP TABLE IF EXISTS `shop`.`CoreCmsMessageCenter`;
DROP TABLE IF EXISTS `shop`.`CoreCmsMessage`;
DROP TABLE IF EXISTS `shop`.`CoreCmsNotice`;
DROP TABLE IF EXISTS `shop`.`CoreCmsOrderItem`;
DROP TABLE IF EXISTS `shop`.`CoreCmsOrderLog`;
DROP TABLE IF EXISTS `shop`.`CoreCmsOrder`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPagesItems`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPages`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPaymentsFile`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPayments`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPinTuanGoods`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPinTuanRecord`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPinTuanRule`;
DROP TABLE IF EXISTS `shop`.`CoreCmsProductsDistribution`;
DROP TABLE IF EXISTS `shop`.`CoreCmsProducts`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPromotionCondition`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPromotionRecord`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPromotionResult`;
DROP TABLE IF EXISTS `shop`.`CoreCmsPromotion`;
DROP TABLE IF EXISTS `shop`.`CoreCmsServiceDescription`;
DROP TABLE IF EXISTS `shop`.`CoreCmsServices`;
DROP TABLE IF EXISTS `shop`.`CoreCmsSetting`;
DROP TABLE IF EXISTS `shop`.`CoreCmsShip`;
DROP TABLE IF EXISTS `shop`.`CoreCmsSms`;
DROP TABLE IF EXISTS `shop`.`CoreCmsStockLog`;
DROP TABLE IF EXISTS `shop`.`CoreCmsStock`;
DROP TABLE IF EXISTS `shop`.`CoreCmsStore`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserBalance`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserBankCard`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserGrade`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserLog`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserPointLog`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserServicesOrder`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserServicesTicketVerificationLog`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserServicesTicket`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserShip`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserTocash`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserToken`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserWeChatInfo`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserWeChatMsgSubscriptionSwitch`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserWeChatMsgSubscription`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUserWeChatMsgTemplate`;
DROP TABLE IF EXISTS `shop`.`CoreCmsUser`;
DROP TABLE IF EXISTS `shop`.`CoreCmsWeixinAuthor`;
DROP TABLE IF EXISTS `shop`.`CoreCmsWeixinMediaMessage`;
DROP TABLE IF EXISTS `shop`.`CoreCmsWeixinMenu`;
DROP TABLE IF EXISTS `shop`.`CoreCmsWeixinMessage`;
DROP TABLE IF EXISTS `shop`.`HangfireAggregatedCounter`;
DROP TABLE IF EXISTS `shop`.`HangfireCounter`;
DROP TABLE IF EXISTS `shop`.`HangfireDistributedLock`;
DROP TABLE IF EXISTS `shop`.`HangfireHash`;
DROP TABLE IF EXISTS `shop`.`HangfireJobParameter`;
DROP TABLE IF EXISTS `shop`.`HangfireJobQueue`;
DROP TABLE IF EXISTS `shop`.`HangfireJobState`;
DROP TABLE IF EXISTS `shop`.`HangfireJob`;
DROP TABLE IF EXISTS `shop`.`HangfireList`;
DROP TABLE IF EXISTS `shop`.`HangfireServer`;
DROP TABLE IF EXISTS `shop`.`HangfireSet`;
DROP TABLE IF EXISTS `shop`.`HangfireState`;
DROP TABLE IF EXISTS `shop`.`SysDictionaryData`;
DROP TABLE IF EXISTS `shop`.`SysDictionary`;
DROP TABLE IF EXISTS `shop`.`SysLoginRecord`;
DROP TABLE IF EXISTS `shop`.`SysMenu`;
DROP TABLE IF EXISTS `shop`.`SysNLogRecords`;
DROP TABLE IF EXISTS `shop`.`SysOperRecord`;
DROP TABLE IF EXISTS `shop`.`SysOrganization`;
DROP TABLE IF EXISTS `shop`.`SysRoleMenu`;
DROP TABLE IF EXISTS `shop`.`SysRole`;
DROP TABLE IF EXISTS `shop`.`SysTaskLog`;
DROP TABLE IF EXISTS `shop`.`SysUserRole`;
DROP TABLE IF EXISTS `shop`.`SysUser`;
DROP TABLE IF EXISTS `shop`.`TempCheckbox`;
CREATE TABLE `CoreCmsAdvertPosition` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '名称',
  `code` varchar(255) NOT NULL COMMENT '位置编码',
  `createTime` datetime DEFAULT NULL COMMENT '添加时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isEnable` bit(1) NOT NULL COMMENT '是否启用',
  `sort` int NOT NULL COMMENT '排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='广告位置表';
CREATE TABLE `CoreCmsAdvertisement` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `positionId` int NOT NULL COMMENT '位置序列',
  `name` varchar(255) NOT NULL COMMENT '广告名称',
  `imageUrl` varchar(255) NOT NULL COMMENT '广告图片id',
  `val` varchar(255) NOT NULL COMMENT '属性值',
  `valDes` varchar(255) DEFAULT NULL COMMENT '属性值说明',
  `sort` int NOT NULL COMMENT '排序',
  `createTime` datetime DEFAULT NULL COMMENT '添加时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `code` varchar(255) DEFAULT NULL COMMENT '广告位置编码',
  `type` int NOT NULL COMMENT '类型',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='广告表';
CREATE TABLE `CoreCmsAgentGoods` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `goodId` int NOT NULL COMMENT '商品序列',
  `goodRefreshTime` datetime DEFAULT NULL COMMENT '商品编辑时间',
  `sortId` int NOT NULL COMMENT '排序',
  `isEnable` bit(1) NOT NULL COMMENT '是否启用',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '最后更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='代理商品池';
CREATE TABLE `CoreCmsAgentGrade` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '等级序列',
  `name` varchar(255) DEFAULT NULL COMMENT '等级名称',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认等级',
  `isAutoUpGrade` bit(1) NOT NULL COMMENT '是否自动升级',
  `defaultSalesPriceType` int NOT NULL COMMENT '价格加成方式',
  `defaultSalesPriceNumber` int NOT NULL COMMENT '价格加成值',
  `sortId` int NOT NULL COMMENT '等级排序',
  `description` varchar(255) DEFAULT NULL COMMENT '等级说明',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='代理商等级设置表';
CREATE TABLE `CoreCmsAgentOrder` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户代理商id',
  `buyUserId` int NOT NULL COMMENT '下单用户id',
  `orderId` varchar(255) DEFAULT NULL COMMENT '订单编号',
  `amount` decimal(10,0) NOT NULL COMMENT '结算金额',
  `isSettlement` int NOT NULL COMMENT '是否结算',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='代理商订单记录表';
CREATE TABLE `CoreCmsAgentProducts` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `agentGoodsId` int NOT NULL COMMENT '关联代理商品池',
  `goodId` int NOT NULL COMMENT '商品序列',
  `productId` int NOT NULL COMMENT '货品序列',
  `productCostPrice` decimal(10,0) NOT NULL COMMENT '货品成本价格',
  `productPrice` decimal(10,0) NOT NULL COMMENT '货品销售价格',
  `agentGradeId` int NOT NULL COMMENT '代理商等级',
  `agentGradePrice` decimal(10,0) NOT NULL COMMENT '代理价格',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='代理货品池';
CREATE TABLE `CoreCmsAgent` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户Id',
  `name` varchar(255) DEFAULT NULL COMMENT '代理商名称',
  `gradeId` int NOT NULL COMMENT '代理商等级',
  `mobile` varchar(255) DEFAULT NULL COMMENT '手机号',
  `weixin` varchar(255) DEFAULT NULL COMMENT '微信号',
  `qq` varchar(255) DEFAULT NULL COMMENT 'qq号',
  `storeName` varchar(255) DEFAULT NULL COMMENT '店铺名称',
  `storeLogo` varchar(255) DEFAULT NULL COMMENT '店铺Logo',
  `storeBanner` varchar(255) DEFAULT NULL COMMENT '店铺Banner',
  `storeDesc` varchar(255) DEFAULT NULL COMMENT '店铺简介',
  `verifyStatus` int NOT NULL COMMENT '审核状态',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `verifyTime` datetime DEFAULT NULL COMMENT '审核时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='代理商表';
CREATE TABLE `CoreCmsApiAccessToken` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '第三方应用名称',
  `code` varchar(255) NOT NULL COMMENT '第三方应用编码',
  `machineCode` varchar(255) NOT NULL COMMENT '易联云终端号',
  `accessToken` varchar(255) NOT NULL COMMENT '访问令牌，API调用时需要，令牌可以重复使用无失效时间，请开发者全局保存',
  `refreshToken` varchar(255) DEFAULT NULL COMMENT '更新access_token所需，有效时间35天',
  `expiresIn` int NOT NULL COMMENT '令牌的有效时间，单位秒 (30天),注：该模式下可忽略此参数',
  `expiressEndTime` datetime DEFAULT NULL COMMENT '有效期截止时间',
  `parameters` varchar(255) DEFAULT NULL COMMENT '其他参数',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='第三方授权记录表';
CREATE TABLE `CoreCmsArea` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '地区ID',
  `parentId` int DEFAULT NULL COMMENT '父级ID',
  `depth` int DEFAULT NULL COMMENT '地区深度',
  `name` varchar(255) DEFAULT NULL COMMENT '地区名称',
  `postalCode` varchar(255) DEFAULT NULL COMMENT '邮编',
  `sort` int NOT NULL COMMENT '地区排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='地区表';
CREATE TABLE `CoreCmsArticleType` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '分类名称',
  `parentId` int NOT NULL COMMENT '父id',
  `sort` int NOT NULL COMMENT '排序 ',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='文章分类表';
CREATE TABLE `CoreCmsArticle` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `title` varchar(255) NOT NULL COMMENT '标题',
  `brief` varchar(255) DEFAULT NULL COMMENT '简介',
  `coverImage` varchar(255) DEFAULT NULL COMMENT '封面图',
  `contentBody` varchar(255) NOT NULL COMMENT '文章内容',
  `typeId` int NOT NULL COMMENT '分类id',
  `sort` int NOT NULL COMMENT '排序',
  `isPub` bit(1) NOT NULL COMMENT '是否发布',
  `isDel` bit(1) DEFAULT NULL COMMENT '是否删除',
  `pv` int NOT NULL COMMENT '访问量',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='文章表';
CREATE TABLE `CoreCmsBillAftersalesImages` (
  `aftersalesId` varchar(255) NOT NULL COMMENT '售后单id',
  `imageUrl` varchar(255) NOT NULL COMMENT '图片地址',
  `sortId` int NOT NULL COMMENT '排序'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品图片关联表';
CREATE TABLE `CoreCmsBillAftersalesItem` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `aftersalesId` varchar(255) NOT NULL COMMENT '售后单id',
  `orderItemsId` int NOT NULL COMMENT '订单明细ID 关联order_items.id',
  `goodsId` int NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(255) DEFAULT NULL COMMENT '货品编码',
  `bn` varchar(255) DEFAULT NULL COMMENT '商品编码',
  `name` varchar(255) DEFAULT NULL COMMENT '商品名称',
  `imageUrl` varchar(255) NOT NULL COMMENT '图片',
  `nums` int NOT NULL COMMENT '数量',
  `addon` varchar(255) DEFAULT NULL COMMENT '货品明细序列号存储',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='售后单明细表';
CREATE TABLE `CoreCmsBillAftersales` (
  `aftersalesId` varchar(255) NOT NULL COMMENT '售后单id',
  `orderId` varchar(255) NOT NULL COMMENT '订单ID',
  `userId` int NOT NULL COMMENT '用户ID',
  `type` int NOT NULL COMMENT '售后类型',
  `refundAmount` decimal(10,0) NOT NULL COMMENT '退款金额',
  `status` int NOT NULL COMMENT '状态',
  `reason` varchar(255) NOT NULL COMMENT '退款原因',
  `mark` varchar(255) DEFAULT NULL COMMENT '卖家备注，如果审核失败了，会显示到前端',
  `createTime` datetime NOT NULL COMMENT '提交时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`aftersalesId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='退货单表';
CREATE TABLE `CoreCmsBillDeliveryItem` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `deliveryId` varchar(255) NOT NULL COMMENT '发货单号 关联bill_delivery.id',
  `goodsId` int NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(255) NOT NULL COMMENT '货品编码',
  `bn` varchar(255) NOT NULL COMMENT '商品编码',
  `name` varchar(255) NOT NULL COMMENT '商品名称',
  `nums` int NOT NULL COMMENT '发货数量',
  `weight` decimal(10,0) NOT NULL COMMENT '重量',
  `addon` varchar(255) DEFAULT NULL COMMENT '货品明细序列号存储',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='发货单详情表';
CREATE TABLE `CoreCmsBillDeliveryOrderRel` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `orderId` varchar(255) NOT NULL COMMENT '订单号',
  `deliveryId` varchar(255) NOT NULL COMMENT '发货单号',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='发货单订单关联表';
CREATE TABLE `CoreCmsBillDelivery` (
  `deliveryId` varchar(255) NOT NULL COMMENT '发货单序列',
  `orderId` varchar(255) DEFAULT NULL COMMENT '订单号',
  `logiCode` varchar(255) DEFAULT NULL COMMENT '物流公司编码',
  `logiNo` varchar(255) DEFAULT NULL COMMENT '物流单号',
  `logiInformation` varchar(255) DEFAULT NULL COMMENT '快递物流信息',
  `logiStatus` bit(1) NOT NULL COMMENT '快递是否不更新',
  `shipAreaId` int NOT NULL COMMENT '收货地区ID',
  `shipAddress` varchar(255) DEFAULT NULL COMMENT '收货详细地址',
  `shipName` varchar(255) DEFAULT NULL COMMENT '收货人姓名',
  `shipMobile` varchar(255) DEFAULT NULL COMMENT '收货电话',
  `status` int NOT NULL COMMENT '状态',
  `memo` varchar(255) DEFAULT NULL COMMENT '备注',
  `confirmTime` datetime DEFAULT NULL COMMENT '确认收货时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`deliveryId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='发货单表';
CREATE TABLE `CoreCmsBillLading` (
  `id` varchar(255) NOT NULL COMMENT '提货单号',
  `orderId` varchar(255) NOT NULL COMMENT '订单号',
  `storeId` int NOT NULL COMMENT '提货门店ID',
  `name` varchar(255) DEFAULT NULL COMMENT '提货人姓名',
  `mobile` varchar(255) DEFAULT NULL COMMENT '提货手机号',
  `clerkId` int NOT NULL COMMENT '处理店员ID',
  `pickUpTime` datetime DEFAULT NULL COMMENT '提货时间',
  `status` bit(1) NOT NULL COMMENT '是否提货',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDel` bit(1) NOT NULL COMMENT '删除时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='提货单表';
CREATE TABLE `CoreCmsBillPaymentsRel` (
  `paymentId` varchar(255) NOT NULL COMMENT '支付单编号',
  `sourceId` varchar(255) NOT NULL COMMENT '资源编号',
  `money` decimal(10,0) NOT NULL COMMENT '金额'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='支付单明细表';
CREATE TABLE `CoreCmsBillPayments` (
  `paymentId` varchar(255) NOT NULL COMMENT '支付单号',
  `money` decimal(10,0) NOT NULL COMMENT '支付金额',
  `userId` int NOT NULL COMMENT '用户ID 关联user.id',
  `type` int NOT NULL COMMENT '单据类型',
  `status` int NOT NULL COMMENT '支付状态',
  `paymentCode` varchar(255) NOT NULL COMMENT '支付类型编码 关联payments.code',
  `ip` varchar(255) NOT NULL COMMENT '支付单生成IP',
  `parameters` varchar(255) DEFAULT NULL COMMENT '支付的时候需要的参数，存的是json格式的一维数组',
  `payedMsg` varchar(255) DEFAULT NULL COMMENT '支付回调后的状态描述',
  `tradeNo` varchar(255) DEFAULT NULL COMMENT '第三方平台交易流水号',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`paymentId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='支付单表';
CREATE TABLE `CoreCmsBillRefund` (
  `refundId` varchar(255) NOT NULL COMMENT '退款单ID',
  `aftersalesId` varchar(255) NOT NULL COMMENT '售后单id',
  `money` decimal(10,0) NOT NULL COMMENT '退款金额',
  `userId` int NOT NULL COMMENT '用户ID 关联user.id',
  `sourceId` varchar(255) NOT NULL COMMENT '资源id，根据type不同而关联不同的表',
  `type` int NOT NULL COMMENT '资源类型1=订单,2充值单',
  `paymentCode` varchar(255) DEFAULT NULL COMMENT '退款支付类型编码 默认原路返回 关联支付单表支付编码',
  `tradeNo` varchar(255) DEFAULT NULL COMMENT '第三方平台交易流水号',
  `status` int NOT NULL COMMENT '状态',
  `memo` varchar(255) DEFAULT NULL COMMENT '退款失败原因',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`refundId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='退款单表';
CREATE TABLE `CoreCmsBillReshipItem` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `reshipId` varchar(255) NOT NULL COMMENT '退款单单id',
  `orderItemsId` int NOT NULL COMMENT '订单明细ID 关联order_items.id',
  `goodsId` int NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(255) DEFAULT NULL COMMENT '货品编码',
  `bn` varchar(255) DEFAULT NULL COMMENT '商品编码',
  `name` varchar(255) DEFAULT NULL COMMENT '商品名称',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '图片',
  `nums` int NOT NULL COMMENT '数量',
  `addon` varchar(255) DEFAULT NULL COMMENT '货品明细序列号存储',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='退货单明细表';
CREATE TABLE `CoreCmsBillReship` (
  `reshipId` varchar(255) NOT NULL COMMENT '退货单号',
  `orderId` varchar(255) NOT NULL COMMENT '订单序列',
  `aftersalesId` varchar(255) NOT NULL COMMENT '售后单序列',
  `userId` int NOT NULL COMMENT '用户ID 关联user.id',
  `logiCode` varchar(255) DEFAULT NULL COMMENT '物流公司编码',
  `logiNo` varchar(255) DEFAULT NULL COMMENT '物流单号',
  `status` int NOT NULL COMMENT '状态',
  `memo` varchar(255) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`reshipId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='退货单表';
CREATE TABLE `CoreCmsBrand` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '品牌ID',
  `name` varchar(255) DEFAULT NULL COMMENT '品牌名称',
  `logoImageUrl` varchar(255) DEFAULT NULL COMMENT '品牌LOGO',
  `sort` int DEFAULT NULL COMMENT '品牌排序',
  `isShow` bit(1) NOT NULL COMMENT '是否显示',
  `createTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='品牌表';
CREATE TABLE `CoreCmsCart` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户序列',
  `productId` int NOT NULL COMMENT '货品序列',
  `nums` int NOT NULL COMMENT '货品数量',
  `type` int NOT NULL COMMENT '购物车类型',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='购物车表';
CREATE TABLE `CoreCmsClerk` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `storeId` int NOT NULL COMMENT '店铺ID',
  `userId` int NOT NULL COMMENT '用户ID',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='店铺店员关联表';
CREATE TABLE `CoreCmsCoupon` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `couponCode` varchar(255) NOT NULL COMMENT '优惠券编码',
  `promotionId` int NOT NULL COMMENT '优惠券id',
  `isUsed` bit(1) NOT NULL COMMENT '是否使用',
  `userId` int NOT NULL COMMENT '谁领取了',
  `usedId` varchar(255) DEFAULT NULL COMMENT '被谁用了',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `remark` varchar(255) DEFAULT NULL COMMENT '说明',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='优惠券表';
CREATE TABLE `CoreCmsDistributionCondition` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `gradeId` int NOT NULL COMMENT '会员等级Id',
  `code` varchar(255) DEFAULT NULL COMMENT '升级条件编码',
  `parameters` varchar(255) DEFAULT NULL COMMENT '其它参数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='分销商等级升级条件';
CREATE TABLE `CoreCmsDistributionGrade` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '等级序列',
  `name` varchar(255) DEFAULT NULL COMMENT '等级名称',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认等级',
  `isAutoUpGrade` bit(1) NOT NULL COMMENT '是否自动升级',
  `sortId` int NOT NULL COMMENT '等级排序',
  `description` varchar(255) DEFAULT NULL COMMENT '等级说明',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='分销商等级设置表';
CREATE TABLE `CoreCmsDistributionOrder` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户分销商id',
  `buyUserId` int NOT NULL COMMENT '下单用户id',
  `orderId` varchar(255) DEFAULT NULL COMMENT '订单编号',
  `amount` decimal(10,0) NOT NULL COMMENT '结算金额',
  `isSettlement` int NOT NULL COMMENT '是否结算',
  `level` int NOT NULL COMMENT '层级',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='分销商订单记录表';
CREATE TABLE `CoreCmsDistributionResult` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `gradeId` int NOT NULL COMMENT '会员等级Id',
  `code` varchar(255) DEFAULT NULL COMMENT '佣金编码',
  `parameters` varchar(255) DEFAULT NULL COMMENT '佣金设置序列化参数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='等级佣金表';
CREATE TABLE `CoreCmsDistribution` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户Id',
  `name` varchar(255) DEFAULT NULL COMMENT '分销商名称',
  `gradeId` int NOT NULL COMMENT '分销等级',
  `mobile` varchar(255) DEFAULT NULL COMMENT '手机号',
  `weixin` varchar(255) DEFAULT NULL COMMENT '微信号',
  `qq` varchar(255) DEFAULT NULL COMMENT 'qq号',
  `storeName` varchar(255) DEFAULT NULL COMMENT '店铺名称',
  `storeLogo` varchar(255) DEFAULT NULL COMMENT '店铺Logo',
  `storeBanner` varchar(255) DEFAULT NULL COMMENT '店铺Banner',
  `storeDesc` varchar(255) DEFAULT NULL COMMENT '店铺简介',
  `verifyStatus` int NOT NULL COMMENT '审核状态',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `verifyTime` datetime DEFAULT NULL COMMENT '审核时间',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='分销商表';
CREATE TABLE `CoreCmsFormItem` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '字段名称',
  `type` varchar(255) DEFAULT NULL COMMENT '字段类型',
  `validationType` varchar(255) DEFAULT NULL COMMENT '验证类型',
  `value` varchar(255) DEFAULT NULL COMMENT '表单值',
  `defaultValue` varchar(255) DEFAULT NULL COMMENT '默认值',
  `formId` int NOT NULL COMMENT '表单id',
  `required` bit(1) NOT NULL COMMENT '是否必填',
  `sort` int NOT NULL COMMENT '排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='表单项表';
CREATE TABLE `CoreCmsFormSubmitDetail` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `submitId` int NOT NULL COMMENT '提交表单id',
  `formId` int NOT NULL COMMENT '表单id',
  `formItemId` int NOT NULL COMMENT '表单项id',
  `formItemName` varchar(255) DEFAULT NULL COMMENT '表单项名称',
  `formItemValue` varchar(255) DEFAULT NULL COMMENT '表单项值',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='提交表单保存大文本值表';
CREATE TABLE `CoreCmsFormSubmit` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `formId` int NOT NULL COMMENT '表单id',
  `formName` varchar(255) DEFAULT NULL COMMENT '表单名称',
  `userId` int NOT NULL COMMENT '会员id',
  `money` decimal(10,0) NOT NULL COMMENT '总金额',
  `payStatus` bit(1) NOT NULL COMMENT '是否支付',
  `status` bit(1) NOT NULL COMMENT '是否处理',
  `feedback` varchar(255) DEFAULT NULL COMMENT '表单反馈',
  `ip` varchar(255) DEFAULT NULL COMMENT '提交人ip',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户对表的提交记录';
CREATE TABLE `CoreCmsForm` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '表单名称',
  `type` int NOT NULL COMMENT '表单类型',
  `sort` int NOT NULL COMMENT '表单排序',
  `images` varchar(255) DEFAULT NULL COMMENT '图集',
  `videoPath` varchar(255) DEFAULT NULL COMMENT '视频地址',
  `description` varchar(255) DEFAULT NULL COMMENT '表单描述',
  `headType` int NOT NULL COMMENT '表头类型',
  `headTypeValue` varchar(255) DEFAULT NULL COMMENT '表单头值',
  `headTypeVideo` varchar(255) DEFAULT NULL COMMENT '表单视频',
  `buttonName` varchar(255) DEFAULT NULL COMMENT '表单提交按钮名称',
  `buttonColor` varchar(255) DEFAULT NULL COMMENT '表单按钮颜色',
  `isLogin` bit(1) NOT NULL COMMENT '是否需要登录',
  `times` int NOT NULL COMMENT '可提交次数',
  `qrcode` varchar(255) DEFAULT NULL COMMENT '二维码图片地址',
  `returnMsg` varchar(255) DEFAULT NULL COMMENT '提交后提示语',
  `endDateTime` datetime NOT NULL COMMENT '结束时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='表单';
CREATE TABLE `CoreCmsGoodsBrowsing` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `goodsId` int NOT NULL COMMENT '商品id 关联goods.id',
  `userId` int NOT NULL COMMENT '用户id',
  `goodsName` varchar(255) NOT NULL COMMENT '商品名称',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `isdel` bit(1) NOT NULL COMMENT '删除标志',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品浏览记录表';
CREATE TABLE `CoreCmsGoodsCategoryExtend` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `goodsId` int DEFAULT NULL COMMENT '商品id',
  `goodsCategroyId` int DEFAULT NULL COMMENT '商品分类id',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品分类扩展表';
CREATE TABLE `CoreCmsGoodsCategory` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `parentId` int NOT NULL COMMENT '上级分类id',
  `name` varchar(255) NOT NULL COMMENT '分类名称',
  `typeId` int NOT NULL COMMENT '类型ID 关联 goods_type.id',
  `sort` int NOT NULL COMMENT '分类排序',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '分类图片ID',
  `isShow` bit(1) NOT NULL COMMENT '是否显示',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品分类';
CREATE TABLE `CoreCmsGoodsCollection` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `goodsId` int NOT NULL COMMENT '商品id 关联goods.id',
  `userId` int NOT NULL COMMENT '用户id',
  `goodsName` varchar(255) NOT NULL COMMENT '商品名称',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品收藏表';
CREATE TABLE `CoreCmsGoodsComment` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `commentId` int NOT NULL COMMENT '父级评价ID',
  `score` int NOT NULL COMMENT '评价1-5星',
  `userId` int NOT NULL COMMENT '评价用户ID',
  `goodsId` int NOT NULL COMMENT '商品ID',
  `orderId` varchar(255) NOT NULL COMMENT '评价订单ID',
  `addon` varchar(255) DEFAULT NULL COMMENT '货品规格序列号存储',
  `images` varchar(255) DEFAULT NULL COMMENT '评价图片逗号分隔最多五张',
  `contentBody` varchar(255) DEFAULT NULL COMMENT '评价内容',
  `sellerContent` varchar(255) DEFAULT NULL COMMENT '商家回复',
  `isDisplay` bit(1) NOT NULL COMMENT '前台显示',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品评价表';
CREATE TABLE `CoreCmsGoodsGrade` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `goodsId` int NOT NULL COMMENT '商品id',
  `gradeId` int NOT NULL COMMENT '会员等级id',
  `gradePrice` decimal(10,0) NOT NULL COMMENT '会员价',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品会员价表';
CREATE TABLE `CoreCmsGoodsImages` (
  `goodsId` int NOT NULL COMMENT '商品ID',
  `imageId` varchar(255) NOT NULL COMMENT '图片ID',
  `sort` int NOT NULL COMMENT '图片排序'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品图片关联表';
CREATE TABLE `CoreCmsGoodsParams` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '参数名称',
  `value` varchar(255) DEFAULT NULL COMMENT '参数值',
  `type` varchar(255) DEFAULT NULL COMMENT '参数类型',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品参数表';
CREATE TABLE `CoreCmsGoodsTypeParams` (
  `paramsId` int NOT NULL COMMENT '商品参数id',
  `typeId` int NOT NULL COMMENT '商品类型id'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品参数类型关系表';
CREATE TABLE `CoreCmsGoodsTypeSpecRel` (
  `specId` int NOT NULL COMMENT '属性ID',
  `typeId` int NOT NULL COMMENT '类型ID'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品类型和属性关联表';
CREATE TABLE `CoreCmsGoodsTypeSpecValue` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `specId` int NOT NULL COMMENT '属性ID 关联goods_type_spec.id',
  `value` varchar(255) NOT NULL COMMENT '属性值',
  `sort` int NOT NULL COMMENT '排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品类型属性值表';
CREATE TABLE `CoreCmsGoodsTypeSpec` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '属性名称',
  `sort` int NOT NULL COMMENT '属性排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品类型属性表';
CREATE TABLE `CoreCmsGoodsType` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '类型名称',
  `parameters` varchar(255) DEFAULT NULL COMMENT '参数序列号数组',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品类型';
CREATE TABLE `CoreCmsGoods` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '商品ID',
  `bn` varchar(255) NOT NULL COMMENT '商品条码',
  `name` varchar(255) NOT NULL COMMENT '商品名称',
  `brief` varchar(255) DEFAULT NULL COMMENT '商品简介',
  `image` varchar(255) DEFAULT NULL COMMENT '缩略图',
  `images` varchar(255) DEFAULT NULL COMMENT '图集',
  `video` varchar(255) DEFAULT NULL COMMENT '视频',
  `productsDistributionType` int NOT NULL COMMENT '佣金分配方式',
  `goodsCategoryId` int NOT NULL COMMENT '商品分类',
  `goodsTypeId` int NOT NULL COMMENT '商品类别',
  `brandId` int NOT NULL COMMENT '品牌',
  `isNomalVirtual` bit(1) NOT NULL COMMENT '是否虚拟商品',
  `isMarketable` bit(1) NOT NULL COMMENT '是否上架',
  `unit` varchar(255) DEFAULT NULL COMMENT '商品单位',
  `intro` varchar(255) DEFAULT NULL COMMENT '商品详情',
  `spesDesc` varchar(255) DEFAULT NULL COMMENT '商品规格序列号存储',
  `parameters` varchar(255) DEFAULT NULL COMMENT '参数序列化',
  `commentsCount` int NOT NULL COMMENT '评论次数',
  `viewCount` int NOT NULL COMMENT '浏览次数',
  `buyCount` int NOT NULL COMMENT '购买次数',
  `uptime` datetime DEFAULT NULL COMMENT '上架时间',
  `downtime` datetime DEFAULT NULL COMMENT '下架时间',
  `sort` int NOT NULL COMMENT '商品排序',
  `labelIds` varchar(255) DEFAULT NULL COMMENT '标签id逗号分隔',
  `newSpec` varchar(255) DEFAULT NULL COMMENT '自定义规格名称',
  `openSpec` int NOT NULL COMMENT '开启规则',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `isRecommend` bit(1) NOT NULL COMMENT '是否推荐',
  `isHot` bit(1) NOT NULL COMMENT '是否热门',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商品表';
CREATE TABLE `CoreCmsImages` (
  `id` varchar(255) NOT NULL COMMENT '图片ID',
  `name` varchar(255) DEFAULT NULL COMMENT '图片名称',
  `url` varchar(255) DEFAULT NULL COMMENT '绝对地址',
  `path` varchar(255) DEFAULT NULL COMMENT '物理地址',
  `type` varchar(255) DEFAULT NULL COMMENT '存储引擎',
  `isDel` bit(1) DEFAULT NULL COMMENT '是否删除',
  `createTime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='图片表';
CREATE TABLE `CoreCmsInvoiceRecord` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '发票抬头',
  `code` varchar(255) NOT NULL COMMENT '发票税号',
  `frequency` int NOT NULL COMMENT '被使用次数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='发票信息记录';
CREATE TABLE `CoreCmsInvoice` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `category` int NOT NULL COMMENT '开票类型',
  `sourceId` varchar(255) DEFAULT NULL COMMENT '资源ID',
  `userId` int NOT NULL COMMENT '所属用户ID',
  `type` int NOT NULL COMMENT '发票类型',
  `title` varchar(255) NOT NULL COMMENT '发票抬头',
  `taxNumber` varchar(255) NOT NULL COMMENT '发票税号',
  `amount` decimal(10,0) NOT NULL COMMENT '发票金额',
  `status` int NOT NULL COMMENT '开票状态',
  `remarks` varchar(255) DEFAULT NULL COMMENT '开票备注',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='发票表';
CREATE TABLE `CoreCmsLabel` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '标签名称',
  `style` varchar(255) DEFAULT NULL COMMENT '标签样式',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='标签表';
CREATE TABLE `CoreCmsLoginLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户id',
  `state` int DEFAULT NULL COMMENT '登录类型',
  `logTime` datetime DEFAULT NULL COMMENT '时间',
  `city` varchar(255) DEFAULT NULL COMMENT '地点城市',
  `ip` varchar(255) DEFAULT NULL COMMENT 'ip地址',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='登录日志';
CREATE TABLE `CoreCmsLogistics` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `logiName` varchar(255) NOT NULL COMMENT '物流公司名称',
  `logiCode` varchar(255) NOT NULL COMMENT '物流公司编码',
  `imgUrl` varchar(255) DEFAULT NULL COMMENT '物流logo',
  `phone` varchar(255) DEFAULT NULL COMMENT '物流电话',
  `url` varchar(255) DEFAULT NULL COMMENT '物流网址',
  `sort` int NOT NULL COMMENT '排序',
  `isDelete` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='物流公司表';
CREATE TABLE `CoreCmsMessageCenter` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `code` varchar(255) NOT NULL COMMENT '编码',
  `description` varchar(255) DEFAULT NULL COMMENT '描述',
  `isSms` bit(1) NOT NULL COMMENT '启用短信',
  `isMessage` bit(1) NOT NULL COMMENT '启用站内消息',
  `isWxTempletMessage` bit(1) NOT NULL COMMENT '启用微信模板消息',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='消息配置表';
CREATE TABLE `CoreCmsMessage` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户id',
  `code` varchar(255) NOT NULL COMMENT '消息编码',
  `parameters` varchar(255) DEFAULT NULL COMMENT '参数',
  `contentBody` varchar(255) DEFAULT NULL COMMENT '内容',
  `status` bit(1) NOT NULL COMMENT '是否查看',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='消息发送表';
CREATE TABLE `CoreCmsNotice` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `title` varchar(255) NOT NULL COMMENT '公告标题',
  `contentBody` varchar(255) NOT NULL COMMENT '公告内容',
  `type` int DEFAULT NULL COMMENT '公告类型',
  `sort` int DEFAULT NULL COMMENT '排序',
  `isDel` bit(1) DEFAULT NULL COMMENT '软删除位  有时间代表已删除',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='公告表';
CREATE TABLE `CoreCmsOrderItem` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序号',
  `orderId` varchar(255) NOT NULL COMMENT '订单ID 关联order.id',
  `goodsId` int NOT NULL COMMENT '商品ID 关联goods.id',
  `productId` int NOT NULL COMMENT '货品ID 关联products.id',
  `sn` varchar(255) DEFAULT NULL COMMENT '货品编码',
  `bn` varchar(255) DEFAULT NULL COMMENT '商品编码',
  `name` varchar(255) NOT NULL COMMENT '商品名称',
  `price` decimal(10,0) NOT NULL COMMENT '货品价格单价',
  `costprice` decimal(10,0) NOT NULL COMMENT '货品成本价单价',
  `mktprice` decimal(10,0) NOT NULL COMMENT '市场价',
  `imageUrl` varchar(255) NOT NULL COMMENT '图片',
  `nums` int NOT NULL COMMENT '数量',
  `amount` decimal(10,0) NOT NULL COMMENT '总价',
  `promotionAmount` decimal(10,0) NOT NULL COMMENT '商品优惠总金额',
  `promotionList` varchar(255) DEFAULT NULL COMMENT '促销信息',
  `weight` decimal(10,0) NOT NULL COMMENT '总重量',
  `sendNums` int NOT NULL COMMENT '发货数量',
  `addon` varchar(255) DEFAULT NULL COMMENT '货品明细序列号存储',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='订单明细表';
CREATE TABLE `CoreCmsOrderLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `orderId` varchar(255) DEFAULT NULL COMMENT '订单ID',
  `userId` int NOT NULL COMMENT '用户ID',
  `type` int NOT NULL COMMENT '类型',
  `msg` varchar(255) DEFAULT NULL COMMENT '描述介绍',
  `data` varchar(255) DEFAULT NULL COMMENT '请求的数据json',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='订单记录表';
CREATE TABLE `CoreCmsOrder` (
  `orderId` varchar(255) NOT NULL COMMENT '订单号',
  `goodsAmount` decimal(10,0) NOT NULL COMMENT '商品总价',
  `payedAmount` decimal(10,0) NOT NULL COMMENT '已支付的金额',
  `orderAmount` decimal(10,0) NOT NULL COMMENT '订单实际销售总额',
  `payStatus` int NOT NULL COMMENT '支付状态',
  `shipStatus` int NOT NULL COMMENT '发货状态',
  `status` int NOT NULL COMMENT '订单状态',
  `orderType` int NOT NULL COMMENT '订单类型',
  `receiptType` int NOT NULL COMMENT '收货方式',
  `paymentCode` varchar(255) DEFAULT NULL COMMENT '支付方式代码',
  `paymentTime` datetime DEFAULT NULL COMMENT '支付时间',
  `logisticsId` int NOT NULL COMMENT '配送方式ID 关联ship.id',
  `logisticsName` varchar(255) DEFAULT NULL COMMENT '配送方式名称',
  `costFreight` decimal(10,0) NOT NULL COMMENT '配送费用',
  `userId` int NOT NULL COMMENT '用户ID 关联user.id',
  `sellerId` int NOT NULL COMMENT '店铺ID 关联seller.id',
  `confirmStatus` int NOT NULL COMMENT '售后状态',
  `confirmTime` datetime DEFAULT NULL COMMENT '确认收货时间',
  `storeId` int NOT NULL COMMENT '自提门店ID，0就是不门店自提',
  `shipAreaId` int NOT NULL COMMENT '收货地区ID',
  `shipAddress` varchar(255) DEFAULT NULL COMMENT '收货详细地址',
  `shipName` varchar(255) DEFAULT NULL COMMENT '收货人姓名',
  `shipMobile` varchar(255) DEFAULT NULL COMMENT '收货电话',
  `weight` decimal(10,0) NOT NULL COMMENT '商品总重量',
  `taxType` int NOT NULL COMMENT '开发票',
  `taxCode` varchar(255) DEFAULT NULL COMMENT '税号',
  `taxTitle` varchar(255) DEFAULT NULL COMMENT '发票抬头',
  `point` int NOT NULL COMMENT '使用积分',
  `pointMoney` decimal(10,0) NOT NULL COMMENT '积分抵扣金额',
  `orderDiscountAmount` decimal(10,0) NOT NULL COMMENT '订单优惠金额',
  `goodsDiscountAmount` decimal(10,0) NOT NULL COMMENT '商品优惠金额',
  `couponDiscountAmount` decimal(10,0) NOT NULL COMMENT '优惠券优惠额度',
  `coupon` varchar(255) DEFAULT NULL COMMENT '优惠券信息',
  `promotionList` varchar(255) DEFAULT NULL COMMENT '优惠信息',
  `memo` varchar(255) DEFAULT NULL COMMENT '买家备注',
  `ip` varchar(255) DEFAULT NULL COMMENT '下单IP',
  `mark` varchar(255) DEFAULT NULL COMMENT '卖家备注',
  `source` int NOT NULL COMMENT '订单来源',
  `isComment` bit(1) NOT NULL COMMENT '是否评论',
  `isdel` bit(1) NOT NULL COMMENT '删除标志',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`orderId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='订单表';
CREATE TABLE `CoreCmsPagesItems` (
  `id` int NOT NULL AUTO_INCREMENT,
  `widgetCode` varchar(255) NOT NULL COMMENT '组件编码',
  `pageCode` varchar(255) NOT NULL COMMENT '页面编码',
  `positionId` int NOT NULL COMMENT '布局位置',
  `sort` int NOT NULL COMMENT '排序，越小越靠前',
  `parameters` varchar(255) DEFAULT NULL COMMENT '组件配置内容',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='单页内容';
CREATE TABLE `CoreCmsPages` (
  `id` int NOT NULL AUTO_INCREMENT,
  `code` varchar(255) DEFAULT NULL COMMENT '可视化区域编码',
  `name` varchar(255) DEFAULT NULL COMMENT '可编辑区域名称',
  `description` varchar(255) DEFAULT NULL COMMENT '描述',
  `layout` int DEFAULT NULL COMMENT '布局样式编码，1，手机端',
  `type` int DEFAULT NULL COMMENT '1手机端，2PC端',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='单页';
CREATE TABLE `CoreCmsPaymentsFile` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '视频ID',
  `name` varchar(255) DEFAULT NULL COMMENT '视频名称',
  `url` varchar(255) DEFAULT NULL COMMENT '绝对地址',
  `path` varchar(255) DEFAULT NULL COMMENT '物理地址',
  `type` varchar(255) DEFAULT NULL COMMENT '存储引擎',
  `fileType` varchar(255) DEFAULT NULL COMMENT '文件类型',
  `isDel` int DEFAULT NULL COMMENT '是否删除',
  `createTime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='文件表';
CREATE TABLE `CoreCmsPayments` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '支付类型名称',
  `code` varchar(255) DEFAULT NULL COMMENT '支付类型编码',
  `isOnline` bit(1) NOT NULL COMMENT '是否线上支付',
  `parameters` varchar(255) DEFAULT NULL COMMENT '参数',
  `sort` int NOT NULL COMMENT '排序',
  `memo` varchar(255) DEFAULT NULL COMMENT '方式描述',
  `isEnable` bit(1) NOT NULL COMMENT '是否启用',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='支付方式表';
CREATE TABLE `CoreCmsPinTuanGoods` (
  `ruleId` int NOT NULL COMMENT '规则表序列',
  `goodsId` int NOT NULL COMMENT '商品序列'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='拼团商品表';
CREATE TABLE `CoreCmsPinTuanRecord` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `teamId` int NOT NULL COMMENT '团序列',
  `userId` int NOT NULL COMMENT '用户序列',
  `ruleId` int NOT NULL COMMENT '规则表序列',
  `goodsId` int NOT NULL COMMENT '商品序列',
  `status` int NOT NULL COMMENT '状态',
  `orderId` varchar(255) NOT NULL COMMENT '订单序列',
  `parameters` varchar(255) DEFAULT NULL COMMENT '拼团人数Json',
  `closeTime` datetime NOT NULL COMMENT '关闭时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='拼团记录表';
CREATE TABLE `CoreCmsPinTuanRule` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '活动名称',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间',
  `peopleNumber` int NOT NULL COMMENT '人数2-10人',
  `significantInterval` int NOT NULL COMMENT '单位分钟',
  `discountAmount` decimal(10,0) NOT NULL COMMENT '优惠金额',
  `maxNums` int NOT NULL COMMENT '每人限购数量',
  `maxGoodsNums` int NOT NULL COMMENT '每个商品活动数量',
  `sort` int NOT NULL COMMENT '排序',
  `isStatusOpen` bit(1) NOT NULL COMMENT '是否开启',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='拼团规则表';
CREATE TABLE `CoreCmsProductsDistribution` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序号',
  `productsId` int NOT NULL COMMENT '货品序列',
  `productsSN` varchar(255) NOT NULL COMMENT '货品货号',
  `levelOne` decimal(10,0) NOT NULL COMMENT '一级佣金',
  `levelTwo` decimal(10,0) NOT NULL COMMENT '二级佣金',
  `levelThree` decimal(10,0) NOT NULL COMMENT '三级佣金',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='货品三级佣金表';
CREATE TABLE `CoreCmsProducts` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '货品序列',
  `goodsId` int NOT NULL COMMENT '商品序列',
  `barcode` varchar(255) DEFAULT NULL COMMENT '商品条码',
  `sn` varchar(255) DEFAULT NULL COMMENT '货品编码',
  `price` decimal(10,0) NOT NULL COMMENT '货品价格',
  `costprice` decimal(10,0) NOT NULL COMMENT '货品成本价',
  `mktprice` decimal(10,0) NOT NULL COMMENT '货品市场价',
  `marketable` bit(1) NOT NULL COMMENT '是否上架',
  `weight` decimal(10,0) NOT NULL COMMENT '重量(千克)',
  `stock` int NOT NULL COMMENT '库存',
  `freezeStock` int NOT NULL COMMENT '冻结库存',
  `spesDesc` varchar(255) DEFAULT NULL COMMENT '规格值',
  `isDefalut` bit(1) NOT NULL COMMENT '是否默认货品',
  `images` varchar(255) DEFAULT NULL COMMENT '规格图片',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='货品表';
CREATE TABLE `CoreCmsPromotionCondition` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `promotionId` int DEFAULT NULL COMMENT '促销ID',
  `code` varchar(255) DEFAULT NULL COMMENT '促销条件编码',
  `parameters` varchar(255) DEFAULT NULL COMMENT '支付配置参数序列号存储',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='促销条件表';
CREATE TABLE `CoreCmsPromotionRecord` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '记录序列',
  `promotionId` int NOT NULL COMMENT '促销序列',
  `userId` int NOT NULL COMMENT '用户Id',
  `goodsId` int NOT NULL COMMENT '商品id',
  `productId` int NOT NULL COMMENT '货品id',
  `orderId` varchar(255) NOT NULL COMMENT '订单id',
  `type` int NOT NULL COMMENT '3团购/4秒杀',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime NOT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='促销活动记录表';
CREATE TABLE `CoreCmsPromotionResult` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `promotionId` int DEFAULT NULL COMMENT '促销ID',
  `code` varchar(255) DEFAULT NULL COMMENT '促销条件编码',
  `parameters` varchar(255) DEFAULT NULL COMMENT '支付配置参数序列号存储',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='促销结果表';
CREATE TABLE `CoreCmsPromotion` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '促销名称',
  `type` int NOT NULL COMMENT '类型',
  `sort` int NOT NULL COMMENT '排序',
  `parameters` varchar(255) DEFAULT NULL COMMENT '其它参数',
  `maxNums` int NOT NULL COMMENT '每人限购数量',
  `maxGoodsNums` int NOT NULL COMMENT '每个商品活动数量',
  `maxRecevieNums` int NOT NULL COMMENT '最大领取数量',
  `startTime` datetime NOT NULL COMMENT '开始时间',
  `endTime` datetime NOT NULL COMMENT '结束时间',
  `isExclusive` bit(1) NOT NULL COMMENT '是否排他',
  `isAutoReceive` bit(1) NOT NULL COMMENT '是否自动领取',
  `isEnable` bit(1) NOT NULL COMMENT '是否开启',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  `effectiveDays` int NOT NULL COMMENT '有效天数',
  `effectiveHours` int NOT NULL COMMENT '有效小时',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='促销表';
CREATE TABLE `CoreCmsServiceDescription` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `title` varchar(255) NOT NULL COMMENT '名称',
  `type` int NOT NULL COMMENT '类型',
  `description` varchar(255) NOT NULL COMMENT '描述',
  `isShow` bit(1) NOT NULL COMMENT '是否展示',
  `sortId` int NOT NULL COMMENT '排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='商城服务说明';
CREATE TABLE `CoreCmsServices` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `title` varchar(255) NOT NULL COMMENT '项目名称',
  `thumbnail` varchar(255) NOT NULL COMMENT '项目缩略图',
  `description` varchar(255) DEFAULT NULL COMMENT '项目概述',
  `contentBody` varchar(255) NOT NULL COMMENT '项目详细说明',
  `allowedMembership` varchar(255) NOT NULL COMMENT '允许购买会员级别',
  `consumableStore` varchar(255) NOT NULL COMMENT '可消费门店',
  `status` int NOT NULL COMMENT '项目状态',
  `maxBuyNumber` int NOT NULL COMMENT '项目重复购买次数',
  `amount` int NOT NULL COMMENT '项目可销售数量',
  `startTime` datetime NOT NULL COMMENT '项目开始时间',
  `endTime` datetime NOT NULL COMMENT '项目截止时间',
  `validityType` int NOT NULL COMMENT '核销有效期类型',
  `validityStartTime` datetime DEFAULT NULL COMMENT '核销开始时间',
  `validityEndTime` datetime DEFAULT NULL COMMENT '核销结束时间',
  `ticketNumber` int NOT NULL COMMENT '核销服务券数量',
  `createTime` datetime NOT NULL COMMENT '项目创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '项目更新时间',
  `money` decimal(10,0) NOT NULL COMMENT '售价',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='服务项目表';
CREATE TABLE `CoreCmsSetting` (
  `sKey` varchar(255) NOT NULL COMMENT '键',
  `sValue` varchar(255) DEFAULT NULL COMMENT '值',
  PRIMARY KEY (`sKey`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='店铺设置表';
CREATE TABLE `CoreCmsShip` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '配送方式名称',
  `isCashOnDelivery` bit(1) NOT NULL COMMENT '是否货到付款',
  `firstUnit` int NOT NULL COMMENT '首重',
  `continueUnit` int NOT NULL COMMENT '续重',
  `isdefaultAreaFee` bit(1) NOT NULL COMMENT '是否按地区设置配送费用',
  `areaType` int NOT NULL COMMENT '地区类型',
  `firstunitPrice` decimal(10,0) NOT NULL COMMENT '首重费用',
  `continueunitPrice` decimal(10,0) NOT NULL COMMENT '续重费用',
  `exp` varchar(255) DEFAULT NULL COMMENT '配送费用计算表达式',
  `logiName` varchar(255) DEFAULT NULL COMMENT '物流公司名称',
  `logiCode` varchar(255) DEFAULT NULL COMMENT '物流公司编码',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  `sort` int NOT NULL COMMENT '配送方式排序',
  `status` int NOT NULL COMMENT '状态1正常2停用',
  `isfreePostage` bit(1) NOT NULL COMMENT '是否包邮',
  `areaFee` varchar(255) DEFAULT NULL COMMENT '地区配送费用',
  `goodsMoney` decimal(10,0) NOT NULL COMMENT '商品总额满多少免运费',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='配送方式表';
CREATE TABLE `CoreCmsSms` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `mobile` varchar(255) NOT NULL COMMENT '手机号码',
  `code` varchar(255) NOT NULL COMMENT '发送编码',
  `parameters` varchar(255) NOT NULL COMMENT '参数',
  `contentBody` varchar(255) NOT NULL COMMENT '内容',
  `ip` varchar(255) NOT NULL COMMENT 'ip',
  `isUsed` bit(1) NOT NULL COMMENT '是否使用',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='短信发送日志';
CREATE TABLE `CoreCmsStockLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `stockId` varchar(255) NOT NULL COMMENT '库存单号',
  `productId` int NOT NULL COMMENT '货品序列',
  `goodsId` int NOT NULL COMMENT '商品序列',
  `nums` int NOT NULL COMMENT '数量',
  `sn` varchar(255) NOT NULL COMMENT '货品编码',
  `bn` varchar(255) NOT NULL COMMENT '商品条码',
  `goodsName` varchar(255) NOT NULL COMMENT '商品名称',
  `spesDesc` varchar(255) NOT NULL COMMENT '货品明细序列号存储',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='库存操作详情表';
CREATE TABLE `CoreCmsStock` (
  `id` varchar(255) NOT NULL COMMENT '序列',
  `type` int NOT NULL COMMENT '操作类型',
  `manager` int NOT NULL COMMENT '操作员',
  `memo` varchar(255) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='库存操作表';
CREATE TABLE `CoreCmsStore` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `storeName` varchar(255) DEFAULT NULL COMMENT '门店名称',
  `mobile` varchar(255) DEFAULT NULL COMMENT '门店电话/手机号',
  `linkMan` varchar(255) DEFAULT NULL COMMENT '门店联系人',
  `logoImage` varchar(255) DEFAULT NULL COMMENT '门店logo',
  `areaId` int NOT NULL COMMENT '门店地区id',
  `address` varchar(255) DEFAULT NULL COMMENT '门店详细地址',
  `coordinate` varchar(255) DEFAULT NULL COMMENT '坐标位置',
  `latitude` varchar(255) DEFAULT NULL COMMENT '纬度',
  `longitude` varchar(255) DEFAULT NULL COMMENT '经度',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  `distance` decimal(10,0) NOT NULL COMMENT '距离',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='门店表';
CREATE TABLE `CoreCmsUserBalance` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户id',
  `type` int NOT NULL COMMENT '类型',
  `money` decimal(10,0) NOT NULL COMMENT '金额',
  `balance` decimal(10,0) NOT NULL COMMENT '余额',
  `sourceId` varchar(255) DEFAULT NULL COMMENT '资源id',
  `memo` varchar(255) DEFAULT NULL COMMENT '描述',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户余额表';
CREATE TABLE `CoreCmsUserBankCard` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id',
  `userId` int NOT NULL COMMENT '用户ID',
  `bankName` varchar(255) DEFAULT NULL COMMENT '银行名称',
  `bankCode` varchar(255) DEFAULT NULL COMMENT '银行缩写',
  `bankAreaId` int NOT NULL COMMENT '账号地区ID',
  `accountBank` varchar(255) DEFAULT NULL COMMENT '开户行',
  `accountName` varchar(255) DEFAULT NULL COMMENT '账户名',
  `cardNumber` varchar(255) DEFAULT NULL COMMENT '卡号',
  `cardType` int NOT NULL COMMENT '银行卡类型',
  `isdefault` bit(1) NOT NULL COMMENT '默认卡',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '删除时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='银行卡信息';
CREATE TABLE `CoreCmsUserGrade` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id',
  `title` varchar(255) NOT NULL COMMENT '标题',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户等级表';
CREATE TABLE `CoreCmsUserLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id',
  `userId` int NOT NULL COMMENT '用户id',
  `state` int DEFAULT NULL COMMENT '状态',
  `parameters` varchar(255) DEFAULT NULL COMMENT '参数',
  `ip` varchar(255) DEFAULT NULL COMMENT 'ip地址',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户日志';
CREATE TABLE `CoreCmsUserPointLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `userId` int NOT NULL COMMENT '用户ID',
  `type` int NOT NULL COMMENT '类型',
  `num` int NOT NULL COMMENT '积分数量',
  `balance` int NOT NULL COMMENT '积分余额',
  `remarks` varchar(255) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户积分记录表';
CREATE TABLE `CoreCmsUserServicesOrder` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `serviceOrderId` varchar(255) NOT NULL COMMENT '服务订单编号',
  `userId` int NOT NULL COMMENT '关联用户',
  `servicesId` int NOT NULL COMMENT '关联服务',
  `isPay` bit(1) NOT NULL COMMENT '是否支付',
  `payTime` datetime DEFAULT NULL COMMENT '支付时间',
  `paymentId` varchar(255) DEFAULT NULL COMMENT '支付单号',
  `status` int NOT NULL COMMENT '状态',
  `createTime` datetime NOT NULL COMMENT '订单创建时间',
  `servicesEndTime` datetime DEFAULT NULL COMMENT '截止服务时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='服务购买表';
CREATE TABLE `CoreCmsUserServicesTicketVerificationLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `storeId` int NOT NULL COMMENT '核销门店id',
  `serviceId` int NOT NULL COMMENT '关联服务',
  `verificationUserId` int NOT NULL COMMENT '核验人',
  `ticketId` int NOT NULL COMMENT '服务券序列',
  `ticketRedeemCode` varchar(255) NOT NULL COMMENT '核验码',
  `verificationTime` datetime NOT NULL COMMENT '核验时间',
  `isDel` bit(1) NOT NULL COMMENT '是否删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='服务券核验日志';
CREATE TABLE `CoreCmsUserServicesTicket` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `serviceOrderId` varchar(255) NOT NULL COMMENT '关联购买订单',
  `securityCode` varchar(36) NOT NULL COMMENT '安全码',
  `redeemCode` varchar(255) NOT NULL COMMENT '兑换码',
  `serviceId` int NOT NULL COMMENT '关联服务项目id',
  `userId` int NOT NULL COMMENT '关联用户id',
  `status` int NOT NULL COMMENT '状态',
  `validityType` int NOT NULL COMMENT '核销有效期类型',
  `validityStartTime` datetime DEFAULT NULL COMMENT '核销开始时间',
  `validityEndTime` datetime DEFAULT NULL COMMENT '核销结束时间',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `isVerification` bit(1) NOT NULL COMMENT '是否核销',
  `verificationTime` datetime DEFAULT NULL COMMENT '核销时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='服务消费券';
CREATE TABLE `CoreCmsUserShip` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户id 关联user.id',
  `areaId` int NOT NULL COMMENT '收货地区ID',
  `address` varchar(255) DEFAULT NULL COMMENT '收货详细地址',
  `name` varchar(255) DEFAULT NULL COMMENT '收货人姓名',
  `mobile` varchar(255) DEFAULT NULL COMMENT '收货电话',
  `isDefault` bit(1) NOT NULL COMMENT '是否默认',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户地址表';
CREATE TABLE `CoreCmsUserTocash` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT 'id',
  `userId` int NOT NULL COMMENT '用户ID',
  `money` decimal(10,0) NOT NULL COMMENT '提现金额',
  `bankName` varchar(255) DEFAULT NULL COMMENT '银行名称',
  `bankCode` varchar(255) DEFAULT NULL COMMENT '银行缩写',
  `bankAreaId` int DEFAULT NULL COMMENT '账号地区ID',
  `accountBank` varchar(255) DEFAULT NULL COMMENT '开户行',
  `accountName` varchar(255) DEFAULT NULL COMMENT '账户名',
  `cardNumber` varchar(255) DEFAULT NULL COMMENT '卡号',
  `withdrawals` decimal(10,0) NOT NULL COMMENT '提现服务费',
  `status` int NOT NULL COMMENT '提现状态',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户提现记录表';
CREATE TABLE `CoreCmsUserToken` (
  `token` varchar(255) NOT NULL,
  `userId` int NOT NULL COMMENT '用户序列',
  `platform` smallint NOT NULL COMMENT '平台类型，1就是默认，2就是微信小程序',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`token`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户token';
CREATE TABLE `CoreCmsUserWeChatInfo` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `type` int DEFAULT NULL COMMENT '第三方登录类型',
  `userId` int NOT NULL COMMENT '关联用户表',
  `openid` varchar(255) DEFAULT NULL COMMENT 'openId',
  `sessionKey` varchar(255) DEFAULT NULL COMMENT '缓存key',
  `unionId` varchar(255) DEFAULT NULL COMMENT 'unionid',
  `avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `nickName` varchar(255) DEFAULT NULL COMMENT '昵称',
  `gender` int NOT NULL COMMENT '性别',
  `language` varchar(255) DEFAULT NULL COMMENT '语言',
  `city` varchar(255) DEFAULT NULL COMMENT '城市',
  `province` varchar(255) DEFAULT NULL COMMENT '省',
  `country` varchar(255) DEFAULT NULL COMMENT '国家',
  `countryCode` varchar(255) DEFAULT NULL COMMENT '手机号码国家编码',
  `mobile` varchar(255) DEFAULT NULL COMMENT '手机号码',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户表';
CREATE TABLE `CoreCmsUserWeChatMsgSubscriptionSwitch` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `userId` int NOT NULL COMMENT '用户Id',
  `isSwitch` bit(1) NOT NULL COMMENT '是否关闭',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户订阅提醒状态';
CREATE TABLE `CoreCmsUserWeChatMsgSubscription` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `templateId` varchar(255) NOT NULL COMMENT '模板Id',
  `userId` int NOT NULL COMMENT '用户Id',
  `type` varchar(255) NOT NULL COMMENT '订阅类型',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='微信订阅消息存储表';
CREATE TABLE `CoreCmsUserWeChatMsgTemplate` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `templateTitle` varchar(255) DEFAULT NULL COMMENT '模板名称',
  `templateDes` varchar(255) DEFAULT NULL COMMENT '模板说明',
  `templateId` varchar(255) DEFAULT NULL COMMENT '模板Id',
  `data01` varchar(255) DEFAULT NULL COMMENT '字段1',
  `data02` varchar(255) DEFAULT NULL COMMENT '字段2',
  `data03` varchar(255) DEFAULT NULL COMMENT '字段3',
  `data04` varchar(255) DEFAULT NULL COMMENT '字段4',
  `data05` varchar(255) DEFAULT NULL COMMENT '字段5',
  `description` varchar(255) DEFAULT NULL COMMENT '描述',
  `sortId` int NOT NULL COMMENT '排序',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='微信小程序消息模板';
CREATE TABLE `CoreCmsUser` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '用户ID',
  `userName` varchar(255) DEFAULT NULL COMMENT '用户名',
  `passWord` varchar(255) DEFAULT NULL COMMENT '密码',
  `mobile` varchar(255) DEFAULT NULL COMMENT '手机号',
  `sex` int NOT NULL COMMENT '性别[1男2女3未知]',
  `birthday` datetime DEFAULT NULL COMMENT '生日',
  `avatarImage` varchar(255) DEFAULT NULL COMMENT '头像',
  `nickName` varchar(255) DEFAULT NULL COMMENT '昵称',
  `balance` decimal(10,0) NOT NULL COMMENT '余额',
  `point` int NOT NULL COMMENT '积分',
  `grade` int NOT NULL COMMENT '用户等级',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updataTime` datetime DEFAULT NULL COMMENT '更新时间',
  `status` int NOT NULL COMMENT '状态[1正常2停用]',
  `parentId` int NOT NULL COMMENT '推荐人',
  `userWx` int NOT NULL COMMENT '关联三方账户',
  `isDelete` bit(1) NOT NULL COMMENT '删除标志 有数据就是删除',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户表';
CREATE TABLE `CoreCmsWeixinAuthor` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `nickName` varchar(255) DEFAULT NULL COMMENT '授权方昵称',
  `headImg` varchar(255) DEFAULT NULL COMMENT '授权方头像',
  `serviceTypeInfo` varchar(255) DEFAULT NULL COMMENT '默认为0',
  `verifyTypeInfo` int DEFAULT NULL COMMENT '授权方认证类型',
  `userName` varchar(255) DEFAULT NULL COMMENT '小程序的原始ID',
  `signature` varchar(255) DEFAULT NULL COMMENT '帐号介绍',
  `principalName` varchar(255) DEFAULT NULL COMMENT '小程序的主体名称',
  `businessInfo` varchar(255) DEFAULT NULL COMMENT '功能的开通状况（0代表未开通，1代表已开通）： open_store:是否开通微信门店功能 open_scan:是否开通微信扫商品功能 open_pay:是否开通微信支付功能 open_card:是否开通微信卡券功能 open_shake:是否开通微信摇一摇功能',
  `qrcodeUrl` varchar(255) DEFAULT NULL COMMENT '二维码图片的URL',
  `authorizationInfo` varchar(255) DEFAULT NULL COMMENT '授权信息',
  `appId` varchar(255) DEFAULT NULL COMMENT '授权方appid',
  `appSecret` varchar(255) DEFAULT NULL COMMENT '授权方AppSecret',
  `miniprograminfo` varchar(255) DEFAULT NULL COMMENT '可根据这个字段判断是否为小程序类型授权,有值为小程序',
  `funcInfo` varchar(255) DEFAULT NULL COMMENT '小程序授权给开发者的权限集列表，ID为17到19时分别代表： 17.帐号管理权限 18.开发管理权限 19.客服消息管理权限 请注意： 1）该字段的返回不会考虑小程序是否具备该权限集的权限（因为可能部分具备）',
  `authorizerRefreshToken` varchar(255) DEFAULT NULL COMMENT '刷新token',
  `authorizerAccessToken` varchar(255) DEFAULT NULL COMMENT 'token',
  `bindType` int DEFAULT NULL COMMENT '绑定类型，1为第三方授权绑定，2为自助绑定',
  `authorType` varchar(255) DEFAULT NULL COMMENT '授权类型，默认b2c',
  `expiresIn` int DEFAULT NULL COMMENT '绑定授权到期时间',
  `createTime` datetime DEFAULT NULL COMMENT '小程序授权时间',
  `updateTime` datetime DEFAULT NULL COMMENT '小程序更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='获取授权方的帐号基本信息表';
CREATE TABLE `CoreCmsWeixinMediaMessage` (
  `id` int NOT NULL AUTO_INCREMENT,
  `title` varchar(255) DEFAULT NULL COMMENT '标题',
  `author` varchar(255) DEFAULT NULL COMMENT '作者',
  `brief` varchar(255) DEFAULT NULL COMMENT '摘要',
  `imageUrl` varchar(255) DEFAULT NULL COMMENT '封面',
  `contentBody` varchar(255) DEFAULT NULL COMMENT '文章详情',
  `url` varchar(255) DEFAULT NULL COMMENT '原文地址',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='微信图文消息表';
CREATE TABLE `CoreCmsWeixinMenu` (
  `id` int NOT NULL AUTO_INCREMENT,
  `menuId` int NOT NULL COMMENT '菜单id',
  `parentId` int NOT NULL COMMENT '父级菜单',
  `name` varchar(255) NOT NULL COMMENT '菜单名称',
  `type` varchar(255) NOT NULL COMMENT '菜单类型',
  `parameters` varchar(255) NOT NULL COMMENT '菜单参数',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='微信公众号菜单表';
CREATE TABLE `CoreCmsWeixinMessage` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) DEFAULT NULL COMMENT '消息名称',
  `type` int DEFAULT NULL COMMENT '消息类型',
  `parameters` varchar(255) DEFAULT NULL COMMENT '消息参数',
  `isAttention` bit(1) DEFAULT NULL COMMENT '是否关注回复',
  `isDefault` bit(1) DEFAULT NULL COMMENT '是否默认回复',
  `isEnable` bit(1) DEFAULT NULL COMMENT '是否启用',
  `createTime` datetime DEFAULT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='微信消息表';
CREATE TABLE `HangfireAggregatedCounter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireCounterAggregated_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireCounter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` int NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HangfireCounter_Key` (`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireDistributedLock` (
  `Resource` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `CreatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireHash` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Field` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireHash_Key_Field` (`Key`,`Field`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireJobParameter` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(40) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` longtext,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireJobParameter_JobId_Name` (`JobId`,`Name`),
  KEY `FK_HangfireJobParameter_Job` (`JobId`),
  CONSTRAINT `FK_HangfireJobParameter_Job` FOREIGN KEY (`JobId`) REFERENCES `HangfireJob` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireJobQueue` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `FetchedAt` datetime(6) DEFAULT NULL,
  `Queue` varchar(50) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `FetchToken` varchar(36) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HangfireJobQueue_QueueAndFetchedAt` (`Queue`,`FetchedAt`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireJobState` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangfireJobState_Job` (`JobId`),
  CONSTRAINT `FK_HangfireJobState_Job` FOREIGN KEY (`JobId`) REFERENCES `HangfireJob` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireJob` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `StateId` int DEFAULT NULL,
  `StateName` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `InvocationData` longtext NOT NULL,
  `Arguments` longtext NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_HangfireJob_StateName` (`StateName`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireList` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` longtext,
  `ExpireAt` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireServer` (
  `Id` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Data` longtext NOT NULL,
  `LastHeartbeat` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireSet` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Key` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Value` varchar(256) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Score` float NOT NULL,
  `ExpireAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_HangfireSet_Key_Value` (`Key`,`Value`)
) ENGINE=InnoDB AUTO_INCREMENT=65 DEFAULT CHARSET=utf8mb3;
CREATE TABLE `HangfireState` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `JobId` int NOT NULL,
  `Name` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  `Reason` varchar(100) CHARACTER SET utf8 COLLATE utf8_general_ci DEFAULT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `Data` longtext,
  PRIMARY KEY (`Id`),
  KEY `FK_HangfireHangFire_State_Job` (`JobId`),
  CONSTRAINT `FK_HangfireHangFire_State_Job` FOREIGN KEY (`JobId`) REFERENCES `HangfireJob` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb3;
CREATE TABLE `SysDictionaryData` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '字典项id',
  `dictId` int DEFAULT NULL COMMENT '字典id',
  `dictDataCode` varchar(255) DEFAULT NULL COMMENT '字典项标识',
  `dictDataName` varchar(255) DEFAULT NULL COMMENT '字典项名称',
  `comments` varchar(255) DEFAULT NULL COMMENT '备注',
  `sortNumber` int NOT NULL COMMENT '排序号',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='数据字典项表';
CREATE TABLE `SysDictionary` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '字典id',
  `dictCode` varchar(255) NOT NULL COMMENT '字典标识',
  `dictName` varchar(255) NOT NULL COMMENT '字典名称',
  `comments` varchar(255) DEFAULT NULL COMMENT '备注',
  `sortNumber` int NOT NULL COMMENT '排序号',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='数据字典表';
CREATE TABLE `SysLoginRecord` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主键',
  `username` varchar(255) NOT NULL COMMENT '用户账号',
  `os` varchar(255) DEFAULT NULL COMMENT '操作系统',
  `device` varchar(255) DEFAULT NULL COMMENT '设备名',
  `browser` varchar(255) DEFAULT NULL COMMENT '浏览器类型',
  `ip` varchar(255) DEFAULT NULL COMMENT 'ip地址',
  `operType` int NOT NULL COMMENT '操作类型',
  `comments` varchar(255) DEFAULT NULL COMMENT '备注',
  `createTime` datetime NOT NULL COMMENT '登录时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb3 COMMENT='登录日志表';
CREATE TABLE `SysMenu` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '菜单id',
  `parentId` int NOT NULL COMMENT '上级id,0是顶级',
  `identificationCode` varchar(255) DEFAULT NULL COMMENT '英文标识符',
  `menuName` varchar(255) DEFAULT NULL COMMENT '菜单名称',
  `menuIcon` varchar(255) DEFAULT NULL COMMENT '菜单图标',
  `path` varchar(255) DEFAULT NULL COMMENT '菜单路由关键字',
  `component` varchar(255) DEFAULT NULL COMMENT '菜单组件地址',
  `menuType` int NOT NULL COMMENT '类型,0菜单,1按钮',
  `sortNumber` int DEFAULT NULL COMMENT '排序号',
  `authority` varchar(255) DEFAULT NULL COMMENT '权限标识',
  `target` varchar(255) DEFAULT NULL COMMENT '打开位置',
  `iconColor` varchar(255) DEFAULT NULL COMMENT '菜单图标颜色',
  `hide` bit(1) NOT NULL COMMENT '是否隐藏,0否,1是',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '更新时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='菜单表';
CREATE TABLE `SysNLogRecords` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `LogDate` datetime DEFAULT NULL COMMENT '时间',
  `LogLevel` varchar(255) DEFAULT NULL COMMENT '级别',
  `LogType` varchar(255) DEFAULT NULL COMMENT '事件日志上下文',
  `LogTitle` varchar(255) DEFAULT NULL COMMENT '事件标题',
  `Logger` varchar(255) DEFAULT NULL COMMENT '记录器名字',
  `Message` varchar(255) DEFAULT NULL COMMENT '消息',
  `Exception` varchar(255) DEFAULT NULL COMMENT '异常信息',
  `MachineName` varchar(255) DEFAULT NULL COMMENT '机器名称',
  `MachineIp` varchar(255) DEFAULT NULL COMMENT 'ip',
  `NetRequestMethod` varchar(255) DEFAULT NULL COMMENT '请求方式',
  `NetRequestUrl` varchar(255) DEFAULT NULL COMMENT '请求地址',
  `NetUserIsauthenticated` varchar(255) DEFAULT NULL COMMENT '是否授权',
  `NetUserAuthtype` varchar(255) DEFAULT NULL COMMENT '授权类型',
  `NetUserIdentity` varchar(255) DEFAULT NULL COMMENT '身份认证',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb3 COMMENT='Nlog记录表';
CREATE TABLE `SysOperRecord` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主键',
  `userId` int NOT NULL COMMENT '用户id',
  `userName` varchar(255) DEFAULT NULL COMMENT '用户名',
  `model` varchar(255) DEFAULT NULL COMMENT '操作模块',
  `description` varchar(255) DEFAULT NULL COMMENT '操作方法',
  `url` varchar(255) DEFAULT NULL COMMENT '请求地址',
  `requestMethod` varchar(255) DEFAULT NULL COMMENT '请求方式',
  `operMethod` varchar(255) DEFAULT NULL COMMENT '调用方法',
  `param` varchar(255) DEFAULT NULL COMMENT '请求参数',
  `result` varchar(255) DEFAULT NULL COMMENT '返回结果',
  `ip` varchar(255) DEFAULT NULL COMMENT 'ip地址',
  `spendTime` varchar(255) NOT NULL COMMENT '请求耗时,单位毫秒',
  `state` int NOT NULL COMMENT '状态,0成功,1异常',
  `createTime` datetime NOT NULL COMMENT '登录时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='操作日志表';
CREATE TABLE `SysOrganization` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '机构id',
  `parentId` int NOT NULL COMMENT '上级id,0是顶级',
  `organizationName` varchar(255) DEFAULT NULL COMMENT '机构名称',
  `organizationFullName` varchar(255) DEFAULT NULL COMMENT '机构名称',
  `organizationType` int NOT NULL COMMENT '机构类型',
  `leaderId` int DEFAULT NULL COMMENT '负责人id',
  `sortNumber` int NOT NULL COMMENT '排序号',
  `comments` varchar(255) DEFAULT NULL COMMENT '备注',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='组织机构表';
CREATE TABLE `SysRoleMenu` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主键',
  `roleId` int NOT NULL COMMENT '角色id',
  `menuId` int NOT NULL COMMENT '菜单id',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='角色菜单关联表';
CREATE TABLE `SysRole` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '角色id',
  `roleName` varchar(255) NOT NULL COMMENT '角色名称',
  `roleCode` varchar(255) DEFAULT NULL COMMENT '角色标识',
  `comments` varchar(255) DEFAULT NULL COMMENT '备注',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='角色表';
CREATE TABLE `SysTaskLog` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '序列',
  `name` varchar(255) NOT NULL COMMENT '任务名称',
  `createTime` datetime NOT NULL COMMENT '完成时间',
  `isSuccess` bit(1) NOT NULL COMMENT '是否完成',
  `parameters` varchar(255) DEFAULT NULL COMMENT '其他数据',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='定时任务日志';
CREATE TABLE `SysUserRole` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '主键',
  `userId` int NOT NULL COMMENT '用户id',
  `roleId` int NOT NULL COMMENT '角色id',
  `createTime` datetime NOT NULL COMMENT '创建时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户角色关联表';
CREATE TABLE `SysUser` (
  `id` int NOT NULL AUTO_INCREMENT COMMENT '用户id',
  `userName` varchar(255) DEFAULT NULL COMMENT '账号',
  `passWord` varchar(255) DEFAULT NULL COMMENT '密码',
  `nickName` varchar(255) DEFAULT NULL COMMENT '昵称',
  `avatar` varchar(255) DEFAULT NULL COMMENT '头像',
  `sex` int NOT NULL COMMENT '性别',
  `phone` varchar(255) DEFAULT NULL COMMENT '手机号',
  `email` varchar(255) DEFAULT NULL COMMENT '邮箱',
  `emailVerified` bit(1) NOT NULL COMMENT '邮箱是否验证',
  `trueName` varchar(255) DEFAULT NULL COMMENT '真实姓名',
  `idCard` varchar(255) DEFAULT NULL COMMENT '身份证号',
  `birthday` datetime DEFAULT NULL COMMENT '出生日期',
  `introduction` varchar(255) DEFAULT NULL COMMENT '个人简介',
  `organizationId` int DEFAULT NULL COMMENT '机构id',
  `state` int NOT NULL COMMENT '状态,0正常,1冻结',
  `deleted` bit(1) NOT NULL COMMENT '是否删除,0否,1是',
  `createTime` datetime NOT NULL COMMENT '注册时间',
  `updateTime` datetime DEFAULT NULL COMMENT '修改时间',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COMMENT='用户表';
CREATE TABLE `TempCheckbox` (
  `checked` bit(1) NOT NULL,
  `value` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAdvertPosition` WRITE;
DELETE FROM `shop`.`CoreCmsAdvertPosition`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAdvertisement` WRITE;
DELETE FROM `shop`.`CoreCmsAdvertisement`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAgentGoods` WRITE;
DELETE FROM `shop`.`CoreCmsAgentGoods`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAgentGrade` WRITE;
DELETE FROM `shop`.`CoreCmsAgentGrade`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAgentOrder` WRITE;
DELETE FROM `shop`.`CoreCmsAgentOrder`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAgentProducts` WRITE;
DELETE FROM `shop`.`CoreCmsAgentProducts`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsAgent` WRITE;
DELETE FROM `shop`.`CoreCmsAgent`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsApiAccessToken` WRITE;
DELETE FROM `shop`.`CoreCmsApiAccessToken`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsArea` WRITE;
DELETE FROM `shop`.`CoreCmsArea`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsArticleType` WRITE;
DELETE FROM `shop`.`CoreCmsArticleType`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsArticle` WRITE;
DELETE FROM `shop`.`CoreCmsArticle`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillAftersalesImages` WRITE;
DELETE FROM `shop`.`CoreCmsBillAftersalesImages`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillAftersalesItem` WRITE;
DELETE FROM `shop`.`CoreCmsBillAftersalesItem`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillAftersales` WRITE;
DELETE FROM `shop`.`CoreCmsBillAftersales`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillDeliveryItem` WRITE;
DELETE FROM `shop`.`CoreCmsBillDeliveryItem`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillDeliveryOrderRel` WRITE;
DELETE FROM `shop`.`CoreCmsBillDeliveryOrderRel`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillDelivery` WRITE;
DELETE FROM `shop`.`CoreCmsBillDelivery`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillLading` WRITE;
DELETE FROM `shop`.`CoreCmsBillLading`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillPaymentsRel` WRITE;
DELETE FROM `shop`.`CoreCmsBillPaymentsRel`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillPayments` WRITE;
DELETE FROM `shop`.`CoreCmsBillPayments`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillRefund` WRITE;
DELETE FROM `shop`.`CoreCmsBillRefund`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillReshipItem` WRITE;
DELETE FROM `shop`.`CoreCmsBillReshipItem`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBillReship` WRITE;
DELETE FROM `shop`.`CoreCmsBillReship`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsBrand` WRITE;
DELETE FROM `shop`.`CoreCmsBrand`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsCart` WRITE;
DELETE FROM `shop`.`CoreCmsCart`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsClerk` WRITE;
DELETE FROM `shop`.`CoreCmsClerk`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsCoupon` WRITE;
DELETE FROM `shop`.`CoreCmsCoupon`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsDistributionCondition` WRITE;
DELETE FROM `shop`.`CoreCmsDistributionCondition`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsDistributionGrade` WRITE;
DELETE FROM `shop`.`CoreCmsDistributionGrade`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsDistributionOrder` WRITE;
DELETE FROM `shop`.`CoreCmsDistributionOrder`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsDistributionResult` WRITE;
DELETE FROM `shop`.`CoreCmsDistributionResult`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsDistribution` WRITE;
DELETE FROM `shop`.`CoreCmsDistribution`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsFormItem` WRITE;
DELETE FROM `shop`.`CoreCmsFormItem`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsFormSubmitDetail` WRITE;
DELETE FROM `shop`.`CoreCmsFormSubmitDetail`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsFormSubmit` WRITE;
DELETE FROM `shop`.`CoreCmsFormSubmit`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsForm` WRITE;
DELETE FROM `shop`.`CoreCmsForm`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsBrowsing` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsBrowsing`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsCategoryExtend` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsCategoryExtend`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsCategory` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsCategory`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsCollection` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsCollection`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsComment` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsComment`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsGrade` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsGrade`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsImages` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsImages`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsParams` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsParams`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsTypeParams` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsTypeParams`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsTypeSpecRel` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsTypeSpecRel`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsTypeSpecValue` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsTypeSpecValue`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsTypeSpec` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsTypeSpec`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoodsType` WRITE;
DELETE FROM `shop`.`CoreCmsGoodsType`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsGoods` WRITE;
DELETE FROM `shop`.`CoreCmsGoods`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsImages` WRITE;
DELETE FROM `shop`.`CoreCmsImages`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsInvoiceRecord` WRITE;
DELETE FROM `shop`.`CoreCmsInvoiceRecord`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsInvoice` WRITE;
DELETE FROM `shop`.`CoreCmsInvoice`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsLabel` WRITE;
DELETE FROM `shop`.`CoreCmsLabel`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsLoginLog` WRITE;
DELETE FROM `shop`.`CoreCmsLoginLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsLogistics` WRITE;
DELETE FROM `shop`.`CoreCmsLogistics`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsMessageCenter` WRITE;
DELETE FROM `shop`.`CoreCmsMessageCenter`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsMessage` WRITE;
DELETE FROM `shop`.`CoreCmsMessage`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsNotice` WRITE;
DELETE FROM `shop`.`CoreCmsNotice`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsOrderItem` WRITE;
DELETE FROM `shop`.`CoreCmsOrderItem`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsOrderLog` WRITE;
DELETE FROM `shop`.`CoreCmsOrderLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsOrder` WRITE;
DELETE FROM `shop`.`CoreCmsOrder`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPagesItems` WRITE;
DELETE FROM `shop`.`CoreCmsPagesItems`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPages` WRITE;
DELETE FROM `shop`.`CoreCmsPages`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPaymentsFile` WRITE;
DELETE FROM `shop`.`CoreCmsPaymentsFile`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPayments` WRITE;
DELETE FROM `shop`.`CoreCmsPayments`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPinTuanGoods` WRITE;
DELETE FROM `shop`.`CoreCmsPinTuanGoods`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPinTuanRecord` WRITE;
DELETE FROM `shop`.`CoreCmsPinTuanRecord`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPinTuanRule` WRITE;
DELETE FROM `shop`.`CoreCmsPinTuanRule`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsProductsDistribution` WRITE;
DELETE FROM `shop`.`CoreCmsProductsDistribution`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsProducts` WRITE;
DELETE FROM `shop`.`CoreCmsProducts`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPromotionCondition` WRITE;
DELETE FROM `shop`.`CoreCmsPromotionCondition`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPromotionRecord` WRITE;
DELETE FROM `shop`.`CoreCmsPromotionRecord`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPromotionResult` WRITE;
DELETE FROM `shop`.`CoreCmsPromotionResult`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsPromotion` WRITE;
DELETE FROM `shop`.`CoreCmsPromotion`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsServiceDescription` WRITE;
DELETE FROM `shop`.`CoreCmsServiceDescription`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsServices` WRITE;
DELETE FROM `shop`.`CoreCmsServices`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsSetting` WRITE;
DELETE FROM `shop`.`CoreCmsSetting`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsShip` WRITE;
DELETE FROM `shop`.`CoreCmsShip`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsSms` WRITE;
DELETE FROM `shop`.`CoreCmsSms`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsStockLog` WRITE;
DELETE FROM `shop`.`CoreCmsStockLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsStock` WRITE;
DELETE FROM `shop`.`CoreCmsStock`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsStore` WRITE;
DELETE FROM `shop`.`CoreCmsStore`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserBalance` WRITE;
DELETE FROM `shop`.`CoreCmsUserBalance`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserBankCard` WRITE;
DELETE FROM `shop`.`CoreCmsUserBankCard`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserGrade` WRITE;
DELETE FROM `shop`.`CoreCmsUserGrade`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserLog` WRITE;
DELETE FROM `shop`.`CoreCmsUserLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserPointLog` WRITE;
DELETE FROM `shop`.`CoreCmsUserPointLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserServicesOrder` WRITE;
DELETE FROM `shop`.`CoreCmsUserServicesOrder`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserServicesTicketVerificationLog` WRITE;
DELETE FROM `shop`.`CoreCmsUserServicesTicketVerificationLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserServicesTicket` WRITE;
DELETE FROM `shop`.`CoreCmsUserServicesTicket`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserShip` WRITE;
DELETE FROM `shop`.`CoreCmsUserShip`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserTocash` WRITE;
DELETE FROM `shop`.`CoreCmsUserTocash`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserToken` WRITE;
DELETE FROM `shop`.`CoreCmsUserToken`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserWeChatInfo` WRITE;
DELETE FROM `shop`.`CoreCmsUserWeChatInfo`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserWeChatMsgSubscriptionSwitch` WRITE;
DELETE FROM `shop`.`CoreCmsUserWeChatMsgSubscriptionSwitch`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserWeChatMsgSubscription` WRITE;
DELETE FROM `shop`.`CoreCmsUserWeChatMsgSubscription`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUserWeChatMsgTemplate` WRITE;
DELETE FROM `shop`.`CoreCmsUserWeChatMsgTemplate`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsUser` WRITE;
DELETE FROM `shop`.`CoreCmsUser`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsWeixinAuthor` WRITE;
DELETE FROM `shop`.`CoreCmsWeixinAuthor`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsWeixinMediaMessage` WRITE;
DELETE FROM `shop`.`CoreCmsWeixinMediaMessage`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsWeixinMenu` WRITE;
DELETE FROM `shop`.`CoreCmsWeixinMenu`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`CoreCmsWeixinMessage` WRITE;
DELETE FROM `shop`.`CoreCmsWeixinMessage`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireAggregatedCounter` WRITE;
DELETE FROM `shop`.`HangfireAggregatedCounter`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireCounter` WRITE;
DELETE FROM `shop`.`HangfireCounter`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireDistributedLock` WRITE;
DELETE FROM `shop`.`HangfireDistributedLock`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireHash` WRITE;
DELETE FROM `shop`.`HangfireHash`;
INSERT INTO `shop`.`HangfireHash` (`Id`,`Key`,`Field`,`Value`,`ExpireAt`) VALUES (1, 'recurring-job:AutoCancelOrderJob.Execute', 'Queue', 'default', NULL),(2, 'recurring-job:AutoCancelOrderJob.Execute', 'Cron', '0 0/5 * * * ? ', NULL),(3, 'recurring-job:AutoCancelOrderJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(4, 'recurring-job:AutoCancelOrderJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.AutoCancelOrderJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(5, 'recurring-job:AutoCancelOrderJob.Execute', 'CreatedAt', '2021-06-18T07:06:12.4161568Z', NULL),(6, 'recurring-job:AutoCancelOrderJob.Execute', 'NextExecution', '2021-06-18T07:10:00.0000000Z', NULL),(7, 'recurring-job:AutoCancelOrderJob.Execute', 'V', '2', NULL),(8, 'recurring-job:CompleteOrderJob.Execute', 'Queue', 'default', NULL),(9, 'recurring-job:CompleteOrderJob.Execute', 'Cron', '0 * * * *', NULL),(10, 'recurring-job:CompleteOrderJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(11, 'recurring-job:CompleteOrderJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.CompleteOrderJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(12, 'recurring-job:CompleteOrderJob.Execute', 'CreatedAt', '2021-06-18T07:06:13.0372664Z', NULL),(13, 'recurring-job:CompleteOrderJob.Execute', 'NextExecution', '2021-06-18T08:00:00.0000000Z', NULL),(14, 'recurring-job:CompleteOrderJob.Execute', 'V', '2', NULL),(15, 'recurring-job:EvaluateOrderJob.Execute', 'Queue', 'default', NULL),(16, 'recurring-job:EvaluateOrderJob.Execute', 'Cron', '0 * * * *', NULL),(17, 'recurring-job:EvaluateOrderJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(18, 'recurring-job:EvaluateOrderJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.EvaluateOrderJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(19, 'recurring-job:EvaluateOrderJob.Execute', 'CreatedAt', '2021-06-18T07:06:13.3034587Z', NULL),(20, 'recurring-job:EvaluateOrderJob.Execute', 'NextExecution', '2021-06-18T08:00:00.0000000Z', NULL),(21, 'recurring-job:EvaluateOrderJob.Execute', 'V', '2', NULL),(22, 'recurring-job:AutoSignOrderJob.Execute', 'Queue', 'default', NULL),(23, 'recurring-job:AutoSignOrderJob.Execute', 'Cron', '0 * * * *', NULL),(24, 'recurring-job:AutoSignOrderJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(25, 'recurring-job:AutoSignOrderJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.AutoSignOrderJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(26, 'recurring-job:AutoSignOrderJob.Execute', 'CreatedAt', '2021-06-18T07:06:13.5701710Z', NULL),(27, 'recurring-job:AutoSignOrderJob.Execute', 'NextExecution', '2021-06-18T08:00:00.0000000Z', NULL),(28, 'recurring-job:AutoSignOrderJob.Execute', 'V', '2', NULL),(29, 'recurring-job:RemindOrderPayJob.Execute', 'Queue', 'default', NULL),(30, 'recurring-job:RemindOrderPayJob.Execute', 'Cron', '0 0/5 * * * ? ', NULL),(31, 'recurring-job:RemindOrderPayJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(32, 'recurring-job:RemindOrderPayJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.RemindOrderPayJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(33, 'recurring-job:RemindOrderPayJob.Execute', 'CreatedAt', '2021-06-18T07:06:13.8363192Z', NULL),(34, 'recurring-job:RemindOrderPayJob.Execute', 'NextExecution', '2021-06-18T07:10:00.0000000Z', NULL),(35, 'recurring-job:RemindOrderPayJob.Execute', 'V', '2', NULL),(36, 'recurring-job:AutoCanclePinTuanJob.Execute', 'Queue', 'default', NULL),(37, 'recurring-job:AutoCanclePinTuanJob.Execute', 'Cron', '* * * * *', NULL),(38, 'recurring-job:AutoCanclePinTuanJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(39, 'recurring-job:AutoCanclePinTuanJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.AutoCanclePinTuanJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(40, 'recurring-job:AutoCanclePinTuanJob.Execute', 'CreatedAt', '2021-06-18T07:06:14.1016417Z', NULL),(41, 'recurring-job:AutoCanclePinTuanJob.Execute', 'NextExecution', '2021-06-18T07:08:00.0000000Z', NULL),(42, 'recurring-job:AutoCanclePinTuanJob.Execute', 'V', '2', NULL),(43, 'recurring-job:RemoveOperationLogJob.Execute', 'Queue', 'default', NULL),(44, 'recurring-job:RemoveOperationLogJob.Execute', 'Cron', '0 0 5 * * ? ', NULL),(45, 'recurring-job:RemoveOperationLogJob.Execute', 'TimeZoneId', 'China Standard Time', NULL),(46, 'recurring-job:RemoveOperationLogJob.Execute', 'Job', '{\"Type\":\"CoreCms.Net.Task.RemoveOperationLogJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', NULL),(47, 'recurring-job:RemoveOperationLogJob.Execute', 'CreatedAt', '2021-06-18T07:06:14.3727743Z', NULL),(48, 'recurring-job:RemoveOperationLogJob.Execute', 'NextExecution', '2021-06-18T21:00:00.0000000Z', NULL),(49, 'recurring-job:RemoveOperationLogJob.Execute', 'V', '2', NULL),(50, 'recurring-job:AutoCanclePinTuanJob.Execute', 'LastExecution', '2021-06-18T07:07:00.0168477Z', NULL),(52, 'recurring-job:AutoCanclePinTuanJob.Execute', 'LastJobId', '1', NULL);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireJobParameter` WRITE;
DELETE FROM `shop`.`HangfireJobParameter`;
INSERT INTO `shop`.`HangfireJobParameter` (`Id`,`JobId`,`Name`,`Value`) VALUES (1, 1, 'RecurringJobId', '\"AutoCanclePinTuanJob.Execute\"'),(2, 1, 'Time', '1624000020'),(3, 1, 'CurrentCulture', '\"zh-CN\"'),(4, 1, 'CurrentUICulture', '\"zh-CN\"');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireJobQueue` WRITE;
DELETE FROM `shop`.`HangfireJobQueue`;
INSERT INTO `shop`.`HangfireJobQueue` (`Id`,`JobId`,`FetchedAt`,`Queue`,`FetchToken`) VALUES (1, 1, '2021-06-18 07:07:13.000000', 'default', '06c5f892-94f4-4fe2-b689-7d7992b1bc26');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireJobState` WRITE;
DELETE FROM `shop`.`HangfireJobState`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireJob` WRITE;
DELETE FROM `shop`.`HangfireJob`;
INSERT INTO `shop`.`HangfireJob` (`Id`,`StateId`,`StateName`,`InvocationData`,`Arguments`,`CreatedAt`,`ExpireAt`) VALUES (1, 2, 'Processing', '{\"Type\":\"CoreCms.Net.Task.AutoCanclePinTuanJob, CoreCms.Net.Task, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"Method\":\"Execute\",\"ParameterTypes\":\"[]\",\"Arguments\":\"[]\"}', '[]', '2021-06-18 07:07:00.181522', NULL);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireList` WRITE;
DELETE FROM `shop`.`HangfireList`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireServer` WRITE;
DELETE FROM `shop`.`HangfireServer`;
INSERT INTO `shop`.`HangfireServer` (`Id`,`Data`,`LastHeartbeat`) VALUES ('funshow:32520:4b73eb5e-ad8e-4103-89fd-6f70fae40f23', '{\"WorkerCount\":20,\"Queues\":[\"default\",\"apis\",\"web\",\"recurring\"],\"StartedAt\":\"2021-06-18T07:07:47.5681087Z\"}', '2021-06-18 07:07:47.642122'),('funshow:32700:810694c7-ed42-4eb4-a5ec-89022537bbe1', '{\"WorkerCount\":20,\"Queues\":[\"default\",\"apis\",\"web\",\"recurring\"],\"StartedAt\":\"2021-06-18T07:06:12.3085372Z\"}', '2021-06-18 07:07:13.028043');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireSet` WRITE;
DELETE FROM `shop`.`HangfireSet`;
INSERT INTO `shop`.`HangfireSet` (`Id`,`Key`,`Value`,`Score`,`ExpireAt`) VALUES (1, 'recurring-jobs', 'AutoCancelOrderJob.Execute', 1624000000, NULL),(2, 'recurring-jobs', 'CompleteOrderJob.Execute', 1624000000, NULL),(3, 'recurring-jobs', 'EvaluateOrderJob.Execute', 1624000000, NULL),(4, 'recurring-jobs', 'AutoSignOrderJob.Execute', 1624000000, NULL),(5, 'recurring-jobs', 'RemindOrderPayJob.Execute', 1624000000, NULL),(6, 'recurring-jobs', 'AutoCanclePinTuanJob.Execute', 1624000000, NULL),(7, 'recurring-jobs', 'RemoveOperationLogJob.Execute', 1624050000, NULL);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`HangfireState` WRITE;
DELETE FROM `shop`.`HangfireState`;
INSERT INTO `shop`.`HangfireState` (`Id`,`JobId`,`Name`,`Reason`,`CreatedAt`,`Data`) VALUES (1, 1, 'Enqueued', 'Triggered by recurring job scheduler', '2021-06-18 07:07:00.436717', '{\"EnqueuedAt\":\"2021-06-18T07:07:00.3693693Z\",\"Queue\":\"default\"}'),(2, 1, 'Processing', NULL, '2021-06-18 07:07:13.248304', '{\"StartedAt\":\"2021-06-18T07:07:13.1204400Z\",\"ServerId\":\"funshow:32700:810694c7-ed42-4eb4-a5ec-89022537bbe1\",\"WorkerId\":\"eed69a92-ff06-48d7-b082-e700ea68c278\"}');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysDictionaryData` WRITE;
DELETE FROM `shop`.`SysDictionaryData`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysDictionary` WRITE;
DELETE FROM `shop`.`SysDictionary`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysLoginRecord` WRITE;
DELETE FROM `shop`.`SysLoginRecord`;
INSERT INTO `shop`.`SysLoginRecord` (`id`,`username`,`os`,`device`,`browser`,`ip`,`operType`,`comments`,`createTime`,`updateTime`) VALUES (1, 'coreshop', 'Microsoft Windows 10.0.19042', NULL, 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.72 Safari/537.36 Edg/89.0.774.45', '::1', 1, NULL, '2021-06-18 15:08:12', NULL);
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysMenu` WRITE;
DELETE FROM `shop`.`SysMenu`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysNLogRecords` WRITE;
DELETE FROM `shop`.`SysNLogRecords`;
INSERT INTO `shop`.`SysNLogRecords` (`id`,`LogDate`,`LogLevel`,`LogType`,`LogTitle`,`Logger`,`Message`,`Exception`,`MachineName`,`MachineIp`,`NetRequestMethod`,`NetRequestUrl`,`NetUserIsauthenticated`,`NetUserAuthtype`,`NetUserIdentity`) VALUES (1, '2021-06-18 15:04:52', 'Trace', 'Web', '网站启动', 'logdb', '网站启动成功', '', 'FUNSHOW', '', '', '', '', '', ''),(2, '2021-06-18 15:05:42', 'Trace', 'Web', '网站启动', 'logdb', '网站启动成功', '', 'FUNSHOW', '', '', '', '', '', ''),(3, '2021-06-18 15:06:11', 'Trace', 'Web', '接口启动', 'logdb', '接口启动成功', '', 'FUNSHOW', '', '', '', '', '', ''),(4, '2021-06-18 15:06:28', 'Trace', 'Web', '网站启动', 'logdb', '网站启动成功', '', 'FUNSHOW', '', '', '', '', '', ''),(5, '2021-06-18 15:07:47', 'Trace', 'Web', '接口启动', 'logdb', '接口启动成功', '', 'FUNSHOW', '', '', '', '', '', ''),(6, '2021-06-18 15:08:07', 'Trace', 'Web', '网站启动', 'logdb', '网站启动成功', '', 'FUNSHOW', '', '', '', '', '', '');
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysOperRecord` WRITE;
DELETE FROM `shop`.`SysOperRecord`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysOrganization` WRITE;
DELETE FROM `shop`.`SysOrganization`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysRoleMenu` WRITE;
DELETE FROM `shop`.`SysRoleMenu`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysRole` WRITE;
DELETE FROM `shop`.`SysRole`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysTaskLog` WRITE;
DELETE FROM `shop`.`SysTaskLog`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysUserRole` WRITE;
DELETE FROM `shop`.`SysUserRole`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`SysUser` WRITE;
DELETE FROM `shop`.`SysUser`;
UNLOCK TABLES;
COMMIT;
BEGIN;
LOCK TABLES `shop`.`TempCheckbox` WRITE;
DELETE FROM `shop`.`TempCheckbox`;
UNLOCK TABLES;
COMMIT;
