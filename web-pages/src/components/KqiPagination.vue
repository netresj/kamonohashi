<template>
  <el-col class="pagination" :span="16">
    <el-pagination
      layout="total, sizes, prev, pager, next"
      :total="total"
      :current-page="value.currentPage"
      :page-size="value.currentPageSize"
      :page-sizes="[10, 30, 50, 100, 200]"
      @size-change="handleSizeChange"
      @current-change="currentChange"
    />
  </el-col>
</template>

<script>
export default {
  props: {
    value: {
      type: Object,
      default: () => ({
        currentPageSize: 10,
        currentPage: 1,
      }),
    },
    total: {
      type: Number,
      default: 0,
    },
  },
  methods: {
    // ページのサイズ(表示件数)変更
    async handleSizeChange(pageSize) {
      this.value.currentPageSize = pageSize
      this.value.currentPage = 1
      this.$emit('change')
    },
    async currentChange(page) {
      this.value.currentPage = page
      this.$emit('change')
    },
  },
}
</script>

<style lang="scss" scoped>
.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>
