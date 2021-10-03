import { apiBaseUrl } from '@/common/setting/constVarsHelper.js';
import * as db from './dbHelper.js' //引入common

const showError = error => {
    let errorMsg = '';
    switch (error.status) {
        case 400:
            errorMsg = '请求参数错误';
            break;
        case 401:
            errorMsg = '未授权，请登录';
            break;
        case 403:
            errorMsg = '跨域拒绝访问';
            break;
        case 404:
            errorMsg = `请求地址出错: ${error.config.url}`;
            break;
        case 408:
            errorMsg = '请求超时';
            break;
        case 500:
            errorMsg = '服务器内部错误';
            break;
        case 501:
            errorMsg = '服务未实现';
            break;
        case 502:
            errorMsg = '网关错误';
            break;
        case 503:
            errorMsg = '服务不可用';
            break;
        case 504:
            errorMsg = '网关超时';
            break;
        case 505:
            errorMsg = 'HTTP版本不受支持';
            break;
        default:
            errorMsg = error.msg;
            break;
    }
    uni.showToast({
        title: errorMsg,
        icon: 'none',
        duration: 1000,
        complete: function () {
            setTimeout(function () {
                uni.hideToast();
            },
                1000);
        }
    });
};

// 文件上传
export const uploadFiles = (data, callback) => {
    // 获取用户token
    let userToken = db.get("userToken");
    if (!userToken) {
        this.$store.commit('showLoginTip', true);
        return false;
    };
    uni.chooseImage({
        success: (chooseImageRes) => {
            uni.showLoading({
                title: '上传中...'
            });
            const tempFilePaths = chooseImageRes.tempFilePaths;
            const uploadTask = uni.uploadFile({
                url: apiBaseUrl + '/Api/Common/UploadImages',
                filePath: tempFilePaths[0],
                fileType: 'image',
                name: 'file',
                header: {
                    'Accept': 'application/json',
                    'Content-Type': 'multipart/form-data',
                    'Authorization': 'Bearer ' + userToken
                },
                formData: {
                    'method': 'images.upload',
                    'upfile': tempFilePaths[0]
                },
                success: (uploadFileRes) => {
                    //console.log("交互成功");
                    //console.log(uploadFileRes);
                    callback(JSON.parse(uploadFileRes.data));
                },
                fail: (error) => {
                    console.log("交互失败");
                    console.log(error);
                    if (error && error.response) {
                        showError(error.response);
                    }
                },
                complete: () => {
                    setTimeout(function () {
                        uni.hideLoading();
                    },
                        250);
                }
            });
            //uploadTask.onProgressUpdate((res) => {
            //	console.log('上传进度' + res.progress);
            //	console.log('已经上传的数据长度' + res.totalBytesSent);
            //	console.log('预期需要上传的数据总长度' + res.totalBytesExpectedToSend);

            //	// 测试条件，取消上传任务。
            //	if (res.progress > 50) {
            //		uploadTask.abort();
            //	}
            //});
        }
    });
};

// 上传图片
export const uploadImage = (num, callback) => {

    // 获取用户token
    let userToken = db.get("userToken");
    if (!userToken) {
        this.$store.commit('showLoginTip', true);
        return false;
    };

    uni.chooseImage({
        count: num,
        success: (res) => {
            uni.showLoading({
                title: '上传中...'
            });
            let tempFilePaths = res.tempFilePaths;
            for (var i = 0; i < tempFilePaths.length; i++) {
                uni.uploadFile({
                    url: apiBaseUrl + '/Api/Common/UploadImages',
                    filePath: tempFilePaths[i],
                    fileType: 'image',
                    name: 'file',
                    header: {
                        'Accept': 'application/json',
                        'Content-Type': 'multipart/form-data',
                        'Authorization': 'Bearer ' + userToken
                    },
                    formData: {
                        'method': 'images.upload',
                        'upfile': tempFilePaths[i]
                    },
                    success: (uploadFileRes) => {
                        callback(JSON.parse(uploadFileRes.data));
                    },
                    fail: (error) => {
                        if (error && error.response) {
                            showError(error.response);
                        }
                    },
                    complete: () => {
                        setTimeout(function () {
                            uni.hideLoading();
                        },
                            250);
                    },
                });
            }
        }
    });
};

