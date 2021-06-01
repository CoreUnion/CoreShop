var deepClone = function(obj) {
  let result = Array.isArray(obj) ? [] : {}
  for (let key in obj) {
    if (obj.hasOwnProperty(key)) {
      if (typeof obj[key] === 'object') {
        result[key] = deepClone(obj[key]) //递归复制
      } else {
        result[key] = obj[key]
      }
    }
  }
  return result
}
Vue.component('layout', {
  template: '#layout',
  data() {
    return {
      pageData: [],
      selectWg: {}
    }
  },
  methods: {
    setSelectWg(data) {
      this.selectWg = data
      this.bus.$emit('changeSelectWg', data)
    },
    handleWidgetAdd: function(evt) {
      const newIndex = evt.newIndex
      const elKey = Date.now() + '_' + Math.ceil(Math.random() * 1000000)
      let newObj = deepClone(this.pageData[newIndex])
      newObj.key = this.pageData[newIndex].type + '_' + elKey
      this.$set(this.pageData, newIndex, newObj)
      this.setSelectWg(this.pageData[newIndex])
    },
    handleSelectWidget(index) {
      this.setSelectWg(this.pageData[index])
    },
    handleWidgetDelete(index) {
      if (this.pageData.length - 1 === index) {
        if (index === 0) {
          this.setSelectWg([])
        } else {
          this.setSelectWg(this.pageData[index - 1])
        }
      } else {
        this.setSelectWg(this.pageData[index + 1])
      }
      this.$nextTick(() => {
        this.pageData.splice(index, 1)
      })
    },
    handleWidgetClone(index) {
      let cloneData = deepClone(this.pageData[index])
      cloneData.key =
        this.pageData[index].type +
        '_' +
        Date.now() +
        '_' +
        Math.ceil(Math.random() * 1000000)
      this.pageData.splice(index, 0, cloneData)
      this.$nextTick(() => {
        this.setSelectWg(this.pageData[index + 1])
      })
    }
  },
  mounted() {}
})
Vue.component('layout-config', {
  template: '#layout-config',
  data: function() {
    return {
      selectWg: {}
    }
  },
  mounted() {
    var that = this
    this.bus.$on('changeSelectWg', function(data) {
      that.selectWg = data
    })
  },
  methods: {
    handleSlideRemove(index) {
      this.selectWg.value.splice(index, 1)
    },
    handleAddSlide() {
      this.selectWg.value.push({
        url: '',
        image: ''
      })
    }
  }
})
