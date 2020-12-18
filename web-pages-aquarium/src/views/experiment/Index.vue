<template>
  <div>
    <h2>実験一覧</h2>

    <el-row type="flex" justify="space-between" :gutter="20">
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
      <el-col class="right-top-button" :span="8">
        <el-button
          icon="el-icon-plus"
          type="primary"
          plain
          @click="openCreateDialog()"
        >
          新しくモデルをトレーニングする
        </el-button>
      </el-col>
    </el-row>

    <el-row class="test">
      <el-table
        class="data-table pl-index-table"
        :data="experimentList"
        border
        @row-click="openEditExperiment"
      >
        <el-table-column prop="id" label="ID" width="120px" />
        <el-table-column prop="name" label="名前" width="auto" />
        <el-table-column prop="dataset" label="データセット" width="auto" />
        <el-table-column prop="template" label="テンプレート" width="auto" />

        <el-table-column
          prop="averagePrecisionRate"
          label="平均適合率"
          width="auto"
        />
        <el-table-column
          prop="lastModified"
          label="最終更新日時"
          width="auto"
        />
        <el-table-column prop="status" label="ステータス" width="auto" />
      </el-table>
    </el-row>
    <el-row>
      <kqi-pagination
        v-model="pageStatus"
        :total="total"
        @change="retrieveData"
      />
    </el-row>
    <router-view @cancel="closeDialog" @done="done" @copy="handleCopy" />
  </div>
</template>

<script>
import KqiPagination from '@/components/KqiPagination'
import { createNamespacedHelpers } from 'vuex'
const { mapGetters, mapActions } = createNamespacedHelpers('dataSet')

export default {
  title: '実験',
  components: {
    KqiPagination,
  },
  data() {
    return {
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'name', name: '名前', type: 'text' },
        { prop: 'dataset', name: 'データセット', type: 'text' },
        { prop: 'template', name: 'テンプレート', type: 'text' },
        { prop: 'averagePrecisionRate', name: '平均適合率', type: 'text' },

        { prop: 'lastModified', name: '最終更新日時', type: 'date' },
        { prop: 'status', name: 'ステータス', type: 'text' },
      ],
      tableData: [],
      experimentList: [
        //TODO APIから取得したら消す
        {
          name: 'FLOWER_training',
          dataset: 'FLOWER',
          template: '●●画像分類ver.2',
          averagePrecisionRate: '0.95',
          lastModified: '2020/10/08',
          status: '学習完了',
        },
      ],
    }
  },
  computed: {
    ...mapGetters(['dataSets', 'total']),
  },

  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions(['fetchDataSets']),

    async currentChange(page) {
      this.pageStatus.currentPage = page
      await this.retrieveData()
    },
    async retrieveData() {
      let params = this.searchCondition
      params.page = this.pageStatus.currentPage
      params.perPage = this.pageStatus.currentPageSize
      params.withTotal = true
      await this.fetchDataSets(params)
    },

    async done(type) {
      if (type === 'delete') {
        // 削除時、表示していたページにデータが無くなっている可能性がある。
        // 総数 % ページサイズ === 1の時、残り1の状態で削除したため、currentPageが1で無ければ1つ前のページに戻す
        if (this.total % this.pageStatus.currentPageSize === 1) {
          if (this.pageStatus.currentPage !== 1) {
            this.pageStatus.currentPage -= 1
          }
        }
      }
      this.closeDialog()
      await this.retrieveData()
      this.showSuccessMessage()
    },
    openCreateDialog() {
      this.$router.push('/aquarium/experiment/create')
    },
    openEditExperiment(selectedRow) {
      this.$router.push('/aquarium/experiment/detail/' + selectedRow.id)
    },
    handleCopy(id) {
      this.$router.push('/aquarium/experiment/create/' + id)
    },
    async search() {
      this.pageStatus.currentPage = 1
      await this.retrieveData()
    },
  },
}
</script>

<style lang="scss" scoped>
.right-top-button {
  text-align: right;
}

.search {
  text-align: right;
  padding-top: 10px;
}
.el-table /deep/ .memo-column div.cell {
  white-space: pre-wrap;
}

.pagination /deep/ .el-input {
  text-align: left;
  width: 120px;
}
</style>