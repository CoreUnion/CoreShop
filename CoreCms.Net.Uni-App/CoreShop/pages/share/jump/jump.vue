<template>
    <view>
        <u-toast ref="uToast" /><u-no-network></u-no-network>
    </view>
</template>
<script>
    export default {
        data() {
            return {};
        },
        onLoad(e) {
            if (e.scene) {
                this.deshare(e.scene);
            } else {
                this.gotoIndex();
            }
        },
        methods: {
            deshare(data) {
                this.$u.api.deshare({ code: data }).then(res => {
                    if (res.status) {
                        this.saveInviteCode(res.data.userShareCode); //存储邀请码
                        switch (res.data.page) {
                            case '1': //首页
                                this.gotoIndex(); //正常
                                break;
                            case '2': //商品
                                this.gotoGoods(res.data.params.goodsId); //正常
                                break;
                            case '3': //拼团
                                this.gotoPinTuan(res.data.params.goodsId, res.data.params.teamId);  //正常
                                break;
                            case '4': //店铺邀请
                                this.gotoStore(res.data.params.store);  //正常
                                break;
                            case '5': //文章页面
                                this.gotoArticle(res.data.params.articleId, res.data.params.articleType);  //正常
                                break;
                            case '6': //参团页面
                                this.gotoInvitationGroup(res.data.params.goodsId, res.data.params.groupId, res.data.params.teamId)
                                break;
                            case '7': //自定义页面
                                this.gotoCustom(res.data.params.pageCode);
                                break;
                            case '8': //智能表单
                                this.gotoForm(res.data.params.id)
                                break;
                            case '9': //团购
                                this.gotoGroup(res.data.params.goodsId, res.data.params.groupId); //正常
                                break;
                            case '10': //秒杀
                                this.gotoSeckill(res.data.params.goodsId, res.data.params.groupId); //正常
                                break;
                            case '11': //代理商
                                this.gotoAgentStore(res.data.params.store); //正常
                                break;
                            default:
                                this.gotoIndex();//正常
                                break;
                        }
                    } else {
                        this.$refs.uToast.show({ title: '失败', type: 'error', back: true });
                    }
                });
            },
            //存储邀请码
            saveInviteCode(invite) {
                if (invite && invite != '') {
                    this.$db.set('invitecode', invite);
                }
            },
            //跳转到首页
            gotoIndex() {
                this.$u.route({ type: 'switchTab', url: '/pages/index/default/default' });
            },
            //跳转到商品
            gotoGoods(id) {
                if (id && id != '') {
                    let url = '/pages/goods/goodDetails/goodDetails?id=' + id;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            //跳转到文章
            gotoArticle(id, idType) {
                if (id && id != '') {
                    let url = '/pages/article/details/details?id=' + id + '&idType=' + idType;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            //跳转到拼团
            gotoPinTuan(id, teamId) {
                if (id && id != '') {
                    let url = '/pages/activity/pinTuan/details/details?id=' + id + '&teamId=' + teamId;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            //跳转到团购
            gotoGroup(id, groupId) {
                if (id && id != '') {
                    let url = '/pages/activity/groupBuying/details/details?id=' + id + '&groupId=' + groupId;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            //跳转到秒杀
            gotoSeckill(id, groupId) {
                if (id && id != '') {
                    let url = '/pages/activity/seckill/details/details?id=' + id + '&groupId=' + groupId;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            //跳转到参团
            //todo:: 功能暂无后续开发
            // gotoInvitationGroup(id, groupId, teamId) {
            // 	if(id && id != '' && groupId && groupId != '' && teamId && teamId != ''){
            // 		let url = '/pages/member/order/invitationGroup/invitationGroup?id=' + id + '&groupId=' + groupId + '&teamId=' + teamId;
            // 		this.$u.route({ type: 'redirectTo', url: url });
            // 	}else{
            // 		this.gotoIndex();
            // 	}
            // },
            //跳转到自定义页
            gotoCustom(pageCode) {
                if (pageCode && pageCode != '') {
                    let url = '/pages/index/custom/custom?pageCode=' + pageCode;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            gotoStore(id) {
                if (id && id != '') {
                    let url = '/pages/member/distribution/myStore/myStore?store=' + id;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            gotoAgentStore(id) {
                if (id && id != '') {
                    let url = '/pages/member/agent/myStore/myStore?store=' + id;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            },
            //跳转表单
            gotoForm(id) {
                if (id && id != '') {
                    let url = '/pages/form/details/details?id=' + id;
                    this.$u.route({ type: 'redirectTo', url: url });
                } else {
                    this.gotoIndex();
                }
            }
        }
    };
</script>