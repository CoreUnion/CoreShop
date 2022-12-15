<template>
    <view class="">
        <u-toast ref="uToast" /><u-no-network></u-no-network>
        <u-navbar :title="title"></u-navbar>

        <!-- 条件筛选 -->
        <view class="u-row-center coreshop-bg-white">
            <u-dropdown ref="uDropdown">
                <u-dropdown-item v-model="comprehensiveDataValue" title="综合" :options="comprehensiveData" @change="comprehensive"></u-dropdown-item>
                <u-dropdown-item v-model="priceSortDataValue" title="价格" :options="priceSortData" @change="priceSort"></u-dropdown-item>
                <u-dropdown-item v-model="salesVolumeDataValue" title="销量" :options="salesVolumeData" @change="salesVolume"></u-dropdown-item>
                <u-dropdown-item v-model="current" title="显示" :options="currentData"></u-dropdown-item>
                <u-dropdown-item title="其他">
                    <view class="slot-content coreshop-bg-white">
                        <view class="fliter-c u-padding-10 u-margin-bottom-80 u-padding-bottom-60">
                            <view class="fliter-item">
                                <view class="fliter-item-title">
                                    <view class="fliter-item-title-hd"><view class="fliter-item-title-hd-title">价格区间</view></view>
                                </view>
                                <view class="fliter-i-c">
                                    <view class="fic-item"><input class="fic-item-input" type="number" v-model="sPrice" /></view>
                                    <view class="fic-item-line"></view>
                                    <view class="fic-item"><input class="fic-item-input" type="number" v-model="ePrice" /></view>
                                </view>
                            </view>
                            <view class="fliter-item" v-if="catList.length > 0">
                                <view class="fliter-item-title">
                                    <view class="fliter-item-title-hd"><view class="fliter-item-title-hd-title">分类</view></view>
                                </view>
                                <view class="fliter-i-c">
                                    <view v-for="item in catList" :key="item.goodsCatId" v-if="item.goodsCatId && item.name" @click="selectKey('catList', item.goodsCatId)">
                                        <view class="fic-item" v-if="!item.isSelect">
                                            <view class="fic-item-text two-line">{{ item.name }}</view>
                                        </view>
                                        <view class="fic-item fic-item-active" v-else-if="item.isSelect">
                                            <view class="fic-item-text two-line">{{ item.name }}</view>
                                        </view>
                                    </view>
                                </view>
                            </view>
                            <view class="fliter-item" v-if="brandList.length > 0">
                                <view class="fliter-item-title">
                                    <view class="fliter-item-title-hd"><view class="fliter-item-title-hd-title">品牌</view></view>
                                </view>
                                <view class="fliter-i-c">
                                    <view v-for="item in brandList" :key="item.id" v-if="item.id && item.name" @click="selectKey('brandList', item.id)">
                                        <view class="fic-item" v-if="!item.isSelect">
                                            <view class="fic-item-text two-line">{{ item.name }}</view>
                                        </view>
                                        <view class="fic-item fic-item-active" v-if="item.isSelect">
                                            <view class="fic-item-text two-line">{{ item.name }}</view>
                                        </view>
                                    </view>
                                </view>
                            </view>
                            <view class="fliter-item" v-if="labelList.length > 0">
                                <view class="fliter-item-title">
                                    <view class="fliter-item-title-hd"><view class="fliter-item-title-hd-title">标签</view></view>
                                </view>
                                <view class="fliter-i-c">
                                    <view v-for="item in labelList" :key="item.id" v-if="item.id && item.name" @click="selectKey('labelList', item.id)">
                                        <view class="fic-item" v-if="!item.isSelect">
                                            <view class="fic-item-text two-line">{{ item.name }}</view>
                                        </view>
                                        <view class="fic-item fic-item-active" v-else-if="item.isSelect">
                                            <view class="fic-item-text two-line">{{ item.name }}</view>
                                        </view>
                                    </view>
                                </view>
                            </view>
                        </view>
                        <view class="coreshop-bottomBox">
                            <u-button size="medium" class="coreshop-btn coreshop-btn-square u-margin-10" type="default" @click="filterNo()">取消</u-button>
                            <u-button size="medium" class="coreshop-btn coreshop-btn-square u-margin-10" type="primary" @click="filterOk()">确定</u-button>
                        </view>
                    </view>
                </u-dropdown-item>
            </u-dropdown>
        </view>


        <!-- 商品列表 -->
        <view>
            <scroll-view scroll-y="true" :scroll-into-view="toView" class="scroll-Y" @scrolltolower="lower" enable-back-to-top="true" lower-threshold="45">
                <!-- 表格图片 -->
                <view v-if="current === 0">
                    <view v-if="goodsList.length > 0" class="goodsBox">
                        <u-grid :col="2" :border="false" :align="center">
                            <u-grid-item bg-color="transparent" :custom-style="{padding: '2rpx'}" v-for="item in goodsList" :key="item.id" @click="goGoodsDetail(item.id)">
                                <view class="good_box">
                                    <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                                    <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="item.id"></u-lazy-load>
                                    <view class="good_title u-line-2">
                                        {{item.name}}
                                    </view>
                                    <view class="good-price">
                                        {{item.price}}元 <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span>
                                    </view>
                                    <view class="good-tag-recommend" v-if="item.isRecommend">
                                        推荐
                                    </view>
                                    <view class="good-tag-hot" v-if="item.isHot">
                                        热门
                                    </view>
                                </view>
                            </u-grid-item>
                        </u-grid>
                        <u-loadmore :status="loadStatus" :icon-type="loadIconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                    </view>
                    <!-- 无数据时默认显示 -->
                    <view class="coreshop-emptybox" v-else>
                        <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="当前列表为空" mode="list"></u-empty>
                    </view>
                </view>

                <!-- 列表图片 -->
                <view v-if="current === 1">
                    <view v-if="goodsList.length > 0">
                        <view class="img-list-item" v-for="(item, index) in goodsList" :key="index" @click="goGoodsDetail(item.id)">
                            <view class="good_box">
                                <u-row gutter="5" justify="space-between">
                                    <u-col span="4">
                                        <!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
                                        <u-lazy-load threshold="-150" border-radius="10" :image="item.image" :index="item.id"></u-lazy-load>
                                        <view class="good-tag-recommend2" v-if="item.isRecommend">
                                            推荐
                                        </view>
                                        <view class="good-tag-hot" v-if="item.isHot">
                                            热门
                                        </view>
                                    </u-col>
                                    <u-col span="8">
                                        <view class="contentBody">
                                            <view class="good_title-xl u-line-2  u-padding-left-10 u-padding-right-10">
                                                {{item.name}}
                                            </view>
                                            <view class="good-price u-padding-10">
                                                {{item.price}}元 <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span>
                                            </view>
                                            <view class="good-des u-padding-10" v-if="item.commentsCount > 0">
                                                {{ item.commentsCount }}条评论
                                            </view>
                                            <view class="good-des u-padding-10" v-else-if="item.commentsCount <= 0">
                                                暂无评论
                                            </view>
                                            <u-icon name="shopping-cart" color="#2979ff" size="40" class="btnCart"></u-icon>
                                        </view>
                                    </u-col>
                                </u-row>
                            </view>
                        </view>
                        <u-loadmore :status="loadStatus" :icon-type="loadIconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
                    </view>
                    <view class="coreshop-emptybox" v-else>
                        <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="当前列表为空" mode="list"></u-empty>
                    </view>
                </view>
            </scroll-view>
        </view>
        <u-back-top :scroll-top="scrollTop" :duration="300"></u-back-top>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [goods],
        data() {
            return {
                CustomBar: (this.CustomBar + 6) * 2,
                title: '列表',
                current: 0,
                id: '',
                showView: false,
                goodsList: [],
                minPrice: '',
                maxPrice: '',

                scrollTop: 0,

                loadStatus: 'loadmore',
                loadIconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },

                toView: '',
                searchData: {
                    where: {},
                    limit: 10,
                    page: 1,
                    order: {
                        key: 'sort',
                        sort: 'asc'
                    }
                },
                searchKey: '请输入关键字搜索', //关键词
                alllist: true,
                allgrid: false,
                screents: true,
                screentc: false,
                sPrice: '',
                ePrice: '',
                brandList: [],
                catList: [],
                labelList: [],

                comprehensiveDataValue: 'asc',
                priceSortDataValue: '',
                salesVolumeDataValue: 1,
                comprehensiveData: [{
                    label: '顺序',
                    value: 'asc',
                },
                {
                    label: '倒序',
                    value: 'desc',
                }],
                salesVolumeData: [{
                    label: '从小到大',
                    value: 'asc',
                },
                {
                    label: '从大到小',
                    value: 'desc',
                }],
                priceSortData: [{
                    label: '从小到大',
                    value: 'asc',
                },
                {
                    label: '从大到小',
                    value: 'desc',
                }],
                currentData: [{
                    label: '表格',
                    value: 0,
                },
                {
                    label: '列表',
                    value: 1,
                }],
            };
        },
        onPageScroll(e) {
            this.scrollTop = e.scrollTop;
        },
        //加载执行
        onLoad: function (options) {
            console.log("CustomBar" + this.CustomBar);
            //console.log(options);
            var where = {};
            if (options.id) {
                where.catId = options.id;
            }
            if (options.key) {
                where = {
                    searchName: options.key
                };
                this.searchKey = options.key;
            }
            if (options.type) {
                if (options.type == 'hot') {
                    where = {
                        hot: true
                    };
                }
                if (options.type == 'recommend') {
                    where = {
                        recommend: true
                    };
                }
            }
            if (options.catId) {
                where.catId = options.catId;
            }
            if (options.brandId) {
                where.brandId = options.brandId;
            }
            if (options.hot) {
                where.hot = options.hot;
            }
            if (options.recommend) {
                where.recommend = options.recommend;
            }
            if (options.labelId) {
                where.labelId = options.labelId;
            }
            this.searchData.where = where;
            //this.setSearchData({
            //    where: where
            //});
            this.setSearchData(this.searchData, true);


            this.getGoods();
        },
        onReachBottom() {
            if (this.loadStatus != 'nomore') {
                this.getGoods();
            }
        },
        methods: {
            listGrid() {
                if (this.current == 0) {
                    this.current = 1;
                } else {
                    this.current = 0;
                }
            },
            //设置查询条件
            setSearchData: function (searchData, clear = false) {
                // 深度克隆
                this.searchData = this.$u.deepClone(searchData);

                if (clear) {
                    this.goodsList = [];
                }
            },
            onChangeShowState: function () {
                var _this = this;
                _this.showView = !_this.showView;
            },
            //点击综合排序
            comprehensive: function (value) {
                //console.log('点击综合排序：' + value);
                if (value) {
                    this.searchData.order = {
                        key: 'sort',
                        sort: value
                    };
                    this.searchData.page = 1; //从第一页重新显示
                    this.setSearchData(this.searchData, true);
                    this.getGoods();
                }
            },
            //销量
            salesVolume: function (value) {
                this.priceSortDataValue = '';
                this.searchData.order = {
                    key: 'buyCount',
                    sort: value
                };
                this.searchData.page = 1; //从第一页重新显示
                this.setSearchData(this.searchData, true);
                this.getGoods();
            },
            //价格排序
            priceSort: function (value) {
                this.salesVolumeDataValue = '';
                this.searchData.order = {
                    key: 'price',
                    sort: value
                };
                this.searchData.page = 1; //从第一页重新显示
                this.setSearchData(this.searchData, true);
                this.getGoods();
            },
            //设置查询价格区间
            // 		orderPrice: function(e) {
            // 			var reg = /^[0-9]+(.[0-9]{2})?$/;
            // 			if (!reg.test(e.detail.value)) {
            // 				this.$u.toast('请输入正确金额');
            // 				this.maxPrice = '';
            // 			} else {
            // 				this.maxPrice = e.detail.value;
            // 			}
            // 		},
            //查询价格区间
            // 		searchPrice: function(event) {
            // 			if (
            // 				this.minPrice > 0 &&
            // 				this.maxPrice > 0 &&
            // 				this.minPrice > this.maxPrice
            // 			) {
            // 				app.common.errorToShow('价格区间有误');
            // 				return false;
            // 			}
            //
            // 			this.setSearchData(
            // 				{
            // 					page: 1,
            // 					where: {
            // 						priceFrom: this.minPrice,
            // 						priceTo: this.maxPrice
            // 					}
            // 				},
            // 				true
            // 			);
            // 			this.getGoods();
            // 		},
            //取得商品数据
            getGoods: function () {
                var _this = this;

                _this.$u.api.goodsList(_this.conditions()).then(res => {
                    if (res.status) {
                        if (res.data.className != '') {
                            _this.title = res.data.className;
                        } else {
                            if (res.data.where && res.data.where.searchName && res.data.where.searchName != '') {
                                _this.title = "商品搜索";
                            }
                        }
                        _this.goodsList = _this.goodsList.concat(res.data.list);

                        if (res.data.brands) {
                            for (let i = 0; i < res.data.brands.length; i++) {
                                res.data.brands[i].isSelect = false;
                            }
                            _this.brandList = res.data.brands;
                        }

                        if (res.data.filter) {
                            if (filter.goodsCat) {
                                for (let i = 0; i < filter.goodsCat.length; i++) {
                                    filter.goodsCat[i].isSelect = false;
                                }
                                _this.catList = filter.goodsCat;
                            }
                            if (filter.labelIds) {
                                for (let i = 0; i < filter.labelIds.length; i++) {
                                    filter.labelIds[i].isSelect = false;
                                }
                                _this.labelList = filter.labelIds;
                            }
                        }
                        //console.log(_this.searchData);
                        if (res.data.totalPages > _this.searchData.page) {
                            _this.loadStatus = 'loadmore';
                            _this.searchData.page++;
                        } else {
                            // 数据已加载完毕
                            _this.loadStatus = 'nomore';
                        }

                    }
                });
            },
            //上拉加载
            lower: function () {
                var _this = this;
                _this.toView = 'loading';

                if (!_this.loadingComplete) {
                    _this.setSearchData({
                        page: _this.searchData.page + 1
                    });
                    _this.getGoods();
                }
            },
            listgrid: function () {
                let _this = this;
                if (_this.alllist) {
                    _this.allgrid = true;
                    _this.listgrid = true;
                    _this.alllist = false;
                } else {
                    _this.allgrid = false;
                    _this.listgrid = false;
                    _this.alllist = true;
                }
            },
            // 统一返回筛选条件 查询条件 分页
            conditions() {
                let data = this.searchData;
                var newData = this.$u.deepClone(data);
                if (data.where) {
                    newData.where = JSON.stringify(data.where);
                }
                //把排序换成字符串
                if (data.order) {
                    var sort = data.order.key + ' ' + data.order.sort;
                    if (data.order.key != 'sort') {
                        sort = sort + ',sort asc'; //如果不是综合排序，增加上第二个排序优先级排序
                    }
                    newData.order = sort;
                } else {
                    newData.order = 'sort asc';
                }
                return newData;
            },
            //老搜索
            search() {
                this.setSearchData(
                    {
                        page: 1,
                        where: {
                            searchName: this.keyword
                        }
                    },
                    true
                );
                this.getGoods();
            },
            //去搜索
            goSearch() {
                let pages = getCurrentPages();
                let prevPage = pages[pages.length - 2];
                if (prevPage && prevPage.route) {
                    let search_flag = prevPage.route;
                    if (search_flag == 'pages/search/search') {
                        uni.navigateBack({
                            delta: 1
                        });
                    } else {
                        this.$u.route('/pages/search/search');
                    }
                } else {
                    this.$u.route('/pages/search/search');
                }
            },
            //取消筛选
            filterNo() {
                this.ePrice = '';
                this.sPrice = '';
                for (let i = 0; i < this.catList.length; i++) {
                    this.catList[i].isSelect = false;
                }
                for (let i = 0; i < this.brandList.length; i++) {
                    this.brandList[i].isSelect = false;
                }
                for (let i = 0; i < this.labelList.length; i++) {
                    this.labelList[i].isSelect = false;
                }
                this.filterOk();
                //this.toclose();
            },
            //确认筛选
            filterOk() {
                let data = this.searchData;

                //获取分类
                // data.where.catId = '';
                for (let i = 0; i < this.catList.length; i++) {
                    if (this.catList[i].isSelect) {
                        data.where.catId = this.catList[i].goodsCatId;
                    }
                }

                //获取多个品牌
                let brandIds = '';
                for (let i = 0; i < this.brandList.length; i++) {
                    if (this.brandList[i].isSelect) {
                        brandIds += this.brandList[i].id + ',';
                    }
                }
                if (brandIds) {
                    brandIds = brandIds.substr(0, brandIds.length - 1);
                }
                data.where.brandId = brandIds;

                //获取标签
                data.where.labelId = '';
                for (let i = 0; i < this.labelList.length; i++) {
                    if (this.labelList[i].isSelect) {
                        data.where.labelId = this.labelList[i].id;
                    }
                }

                //价格区间
                data.where.priceFrom = '';
                data.where.priceTo = '';
                if (
                    this.sPrice * 1 < 0 ||
                    (this.ePrice != '' && this.ePrice <= 0) ||
                    this.ePrice * 1 < 0 ||
                    (this.sPrice * 1 > this.ePrice * 1 && this.sPrice != '' && this.ePrice != '')
                ) {
                    this.$u.toast('价格区间有误');
                    return false;
                } else {
                    data.where.priceFrom = this.sPrice;
                    data.where.priceTo = this.ePrice;
                }
                this.searchData.page = 1; //从第一页重新显示
                this.setSearchData(data, true);
                this.getGoods();
                this.closeDropdown();
            },
            //选择
            selectKey(type, id) {
                //分类一次只能选择一个
                if (type == 'catList') {
                    for (let i = 0; i < this.catList.length; i++) {
                        if (this.catList[i].goodsCatId == id) {
                            this.catList[i].isSelect = this.catList[i].isSelect ? false : true;
                        } else {
                            this.catList[i].isSelect = false;
                        }
                    }
                }

                if (type == 'brandList') {
                    for (let i = 0; i < this.brandList.length; i++) {
                        if (this.brandList[i].id == id) {
                            this.brandList[i].isSelect = this.brandList[i].isSelect ? false : true;
                        } else {
                            this.brandList[i].isSelect = false;
                        }
                    }
                }

                if (type == 'labelList') {
                    for (let i = 0; i < this.labelList.length; i++) {
                        if (this.labelList[i].id == id) {
                            this.labelList[i].isSelect = this.labelList[i].isSelect ? false : true;
                        } else {
                            this.labelList[i].isSelect = false;
                        }
                    }
                }
                console.log(this.brandList);
            },
            closeDropdown() {
                this.$refs.uDropdown.close();
            }
        }
    };
</script>

<style scoped lang="scss">
    @import "list.scss";
</style>
