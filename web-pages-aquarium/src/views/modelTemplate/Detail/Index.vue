<template>
  <div>
    <el-row>
      <el-col :span="12"
        ><h2>テンプレート詳細＞ {{ detail.name }}</h2></el-col
      >
      <el-col :span="12" style="padding-top:15px">
        <el-select
          v-model="versionValue"
          placeholder="Select"
          @change="currentChange"
        >
          <el-option
            v-for="item in versions"
            :key="item.value"
            :label="item.version"
            :value="item.id"
          >
          </el-option>
        </el-select>
      </el-col>
    </el-row>
    <el-tabs v-model="activeName">
      <el-tab-pane label="基本設定" name="baseSetting">
        <base-setting v-if="baseForm" v-model="baseForm" />
      </el-tab-pane>
      <el-tab-pane label="前処理" name="preprocessing">
        <preprocessing v-if="preprocForm" v-model="preprocForm" />
      </el-tab-pane>
      <el-tab-pane label="学習と推論" name="train">
        <training v-if="trainingForm" v-model="trainingForm" />
      </el-tab-pane>
      <!--
      <el-tab-pane label="評価" name="evaluation">
        <evaluation v-if="evaluationForm" v-model="evaluationForm" />
      </el-tab-pane>
      -->
    </el-tabs>
    <el-button type="primary" plain @click="submit()">
      更新
    </el-button>
    <el-button plain @click="deleteVersionDialog = true">
      テンプレートバージョン削除
    </el-button>
    <el-button plain @click="deleteDialog = true">
      テンプレート削除
    </el-button>
    <el-dialog title="" :visible.sync="deleteVersionDialog" width="30%">
      <span v-if="versions.length == 1">
        このテンプレートにはバージョン"{{
          versionDetail.version
        }}"しか存在しないため<br />
        テンプレートバージョン削除することができません。<br />
        <br />
        削除をしたい場合はテンプレート削除を選択してください。
      </span>
      <span v-else>
        <span>
          テンプレートバージョン"{{ versionDetail.version }}"を削除しますか？
        </span>
      </span>
      <span v-if="versions.length == 1" slot="footer" class="dialog-footer">
        <el-button @click="deleteVersionDialog = false">OK</el-button>
      </span>
      <span v-else slot="footer" class="dialog-footer">
        <el-button @click="deleteVersionDialog = false">Cancel</el-button>
        <el-button type="primary" @click="deleteTemplateVersion()">
          削除
        </el-button>
      </span>
    </el-dialog>
    <el-dialog title="" :visible.sync="deleteDialog" width="30%">
      <span>
        テンプレートを削除すると全てのテンプレートバージョンが失われます。<br />
        テンプレートを削除しますか？
      </span>

      <span slot="footer" class="dialog-footer">
        <el-button @click="deleteDialog = false">Cancel</el-button>
        <el-button type="primary" @click="deleteTemplate()">
          削除
        </el-button>
      </span>
    </el-dialog>
  </div>
</template>

<script>
import { createNamespacedHelpers } from 'vuex'
import BaseSetting from './BaseSetting'

import ContainerSettings from './ContainerSettings'
const { mapGetters, mapActions } = createNamespacedHelpers('template')

export default {
  title: 'モデルテンプレート',
  components: {
    BaseSetting,
    Preprocessing: ContainerSettings,
    Training: ContainerSettings,
    // Evaluation: ContainerSettings,
  },
  props: {
    id: {
      type: String,
      default: null,
    },
  },
  data() {
    return {
      deleteDialog: false,
      deleteVersionDialog: false,
      iconname: 'pl-plus',
      pageStatus: {
        currentPage: 1,
        currentPageSize: 10,
      },
      searchCondition: {},
      searchConfigs: [
        { prop: 'id', name: 'ID', type: 'number' },
        { prop: 'name', name: 'データセット名', type: 'text' },
        { prop: 'type', name: '種類', type: 'text' },
        { prop: 'totalImageNumber', name: 'イメージの総数', type: 'text' },
        {
          prop: 'labeledImageNumber',
          name: 'ラベル付きのイメージ数',
          type: 'text',
        },
        { prop: 'lastModified', name: '最終更新日時', type: 'date' },
        { prop: 'status', name: 'ステータス', type: 'text' },
      ],
      tableData: [],
      activeName: 'baseSetting',
      preprocForm: null,
      trainingForm: null,
      evaluationForm: null,
      baseForm: null,
      versionValue: null,
      changeFlg: false,
    }
  },
  computed: {
    ...mapGetters(['detail', 'versionDetail', 'total', 'versions']),
  },
  async created() {
    await this.retrieveData()
  },
  methods: {
    ...mapActions([
      'fetchModelTemplate',
      'fetchVersions',
      'fetchDetail',
      'postByIdVersions',
      'post',
      'put',
      'delete',
      'deleteVersion',
    ]),

    async deleteTemplate() {
      //モデルテンプレート削除
      this.deleteDialog = false
      await this.delete(this.id)
      this.$router.push('/aquarium/model-template')
    },

    async deleteTemplateVersion() {
      //モデルテンプレートバージョン削除

      await this.deleteVersion({ id: this.id, versionId: this.versionValue })
      this.deleteVersionDialog = false
      this.versionValue = null
      this.preprocForm = null
      this.trainingForm = null
      this.evaluationForm = null
      this.retrieveData()

      //再描画
      this.$forceUpdate()
      await this.$notify.success({
        type: 'Success',
        message: `バージョンを削除しました。`,
      })
    },
    async retrieveData() {
      await this.fetchModelTemplate(this.id)
      await this.fetchVersions(this.id)
      if (this.versionValue == null) {
        for (let i in this.versions) {
          // 最新版を保持させる
          if (this.versions[i].version == this.detail.latestVersion) {
            this.versionValue = this.versions[i].id
          }
        }
      }

      this.baseForm = {
        name: this.detail.name,
        memo: this.detail.memo,
        accessLevel: this.detail.accessLevel,
        assignedTenants: this.detail.assignedTenants,
      }
      //最新バージョンを取得する
      await this.fetchDetail({
        id: this.id,
        versionId: this.versionValue,
      })

      this.preprocForm = {
        name: 'preprocForm',
        containerImage: { ...this.versionDetail.preprocessContainerImage },
        gitModel: { ...this.versionDetail.preprocessGitModel },
        entryPoint: this.versionDetail.preprocessEntryPoint,
        resource: {
          cpu: this.versionDetail.preprocessCpu,
          memory: this.versionDetail.preprocessMemory,
          gpu: this.versionDetail.preprocessGpu,
        },
      }

      this.trainingForm = {
        name: 'trainingForm',
        containerImage: { ...this.versionDetail.trainingContainerImage },
        gitModel: { ...this.versionDetail.trainingGitModel },
        entryPoint: this.versionDetail.trainingEntryPoint,
        resource: {
          cpu: this.versionDetail.trainingCpu,
          memory: this.versionDetail.trainingMemory,
          gpu: this.versionDetail.trainingGpu,
        },
      }

      this.evaluationForm = {
        name: 'evaluationForms',
        containerImage: { ...this.versionDetail.evaluationContainerImage },
        gitModel: { ...this.versionDetail.evaluationGitModel },
        entryPoint: this.versionDetail.evaluationEntryPoint,
        resource: {
          cpu: this.versionDetail.evaluationCpu,
          memory: this.versionDetail.evaluationMemory,
          gpu: this.versionDetail.evaluationGpu,
        },
      }
      this.changeFlg = true
    },

    async currentChange() {
      if (this.changeFlg == false) {
        return
      }
      this.preprocForm = null
      this.trainingForm = null
      this.evaluationForm = null
      await this.retrieveData()
    },

    async updateBase() {
      const baseParam = {
        name: this.baseForm.name,
        memo: this.baseForm.memo,
        accessLevel: this.baseForm.accessLevel,
      }
      if (
        this.detail.name != baseParam.name ||
        this.detail.memo != baseParam.memo ||
        this.detail.accessLevel != baseParam.accessLevel
      ) {
        //基本情報更新
        await this['put']({ id: this.id, model: baseParam })
        return true
      } else {
        return false
      }
    },

    async submit() {
      // 入力値チェック
      try {
        // 必須項目の入力チェック
        if (
          // テンプレート名
          this.baseForm.name === null ||
          // 公開設定
          this.baseForm.accessLevel === null ||
          // 学習コンテナイメージ設定
          this.trainingForm.containerImage === null ||
          this.trainingForm.containerImage.registry === null ||
          this.trainingForm.containerImage.image === null ||
          this.trainingForm.containerImage.tag === null ||
          // 学習Git設定
          this.trainingForm.gitModel === null ||
          this.trainingForm.gitModel.git === null ||
          this.trainingForm.gitModel.repository === null ||
          this.trainingForm.gitModel.branch === null ||
          // 実行コマンド
          this.trainingForm.entryPoint.entryPoint === null ||
          this.trainingForm.entryPoint.entryPoint === ''
        ) {
          throw '必須項目が入力されていません : テンプレート名、公開設定、学習の設定は必須項目です。'
        }
        // 前処理の項目について入力チェック
        // dockerイメージ、リポジトリ、入力コマンドのいずれかが入力済みなら他の項目も入力必須
        if (
          (this.preprocForm.containerImage.image !== null ||
            this.preprocForm.gitModel.repository !== null ||
            (this.preprocForm.entryPoint !== null &&
              this.preprocForm.entryPoint !== '')) &&
          // コンテナイメージ設定
          (this.preprocForm.containerImage === null ||
            this.preprocForm.containerImage.registry === null ||
            this.preprocForm.containerImage.image === null ||
            this.preprocForm.containerImage.tag === null ||
            // Git設定
            this.preprocForm.gitModel === null ||
            this.preprocForm.gitModel.git === null ||
            this.preprocForm.gitModel.repository === null ||
            this.preprocForm.gitModel.branch === null ||
            // 実行コマンド
            this.preprocForm.entryPoint === null ||
            this.preprocForm.entryPoint === '')
        ) {
          throw '前処理コンテナの設定を確認してください : イメージ、リポジトリ、入力コマンドのいずれかが入力済みなら他の項目も入力必須です。'
        }
      } catch (message) {
        this.$notify.error({
          message: message,
        })
        return
      }

      //基本情報更新
      let updatedBase = await this.updateBase()

      //バージョン情報が変更されていれば更新するフラグ
      let update = false
      if (this.versionDetail.preprocessContainerImage == null) {
        if (
          this.preprocForm.containerImage != null &&
          this.preprocForm.containerImage.registryId != null
        ) {
          update = true
        }
      } else {
        if (
          this.preprocForm.containerImage == null ||
          this.versionDetail.preprocessContainerImage.registryId !=
            this.preprocForm.containerImage.registry.id ||
          this.versionDetail.preprocessContainerImage.image !=
            this.preprocForm.containerImage.image ||
          this.versionDetail.preprocessContainerImage.tag !=
            this.preprocForm.containerImage.tag ||
          this.versionDetail.preprocessContainerImage.token !=
            this.preprocForm.containerImage.token
        ) {
          update = true
        }
      }

      if (this.versionDetail.preprocessGitModel == null) {
        if (
          this.preprocForm.gitModel != null &&
          this.preprocForm.gitModel.git != null
        ) {
          update = true
        }
      } else {
        if (
          this.preprocForm.gitModel == null ||
          this.versionDetail.preprocessGitModel.gitId !=
            this.preprocForm.gitModel.git.id ||
          this.versionDetail.preprocessGitModel.repository !=
            this.preprocForm.gitModel.repository.name ||
          this.versionDetail.preprocessGitModel.branch !=
            this.preprocForm.gitModel.branch.branchName ||
          this.versionDetail.preprocessGitModel.commitId !=
            this.preprocForm.gitModel.commit.commitId ||
          this.versionDetail.preprocessGitModel.token !=
            this.preprocForm.gitModel.token
        ) {
          update = true
        }
      }
      if (
        this.versionDetail.preprocessEntryPoint !=
          this.preprocForm.entryPoint ||
        this.versionDetail.preprocessCpu != this.preprocForm.resource.cpu ||
        this.versionDetail.preprocessMemory !=
          this.preprocForm.resource.memory ||
        this.versionDetail.preprocessGpu != this.preprocForm.resource.gpu
      ) {
        update = true
      }

      if (
        this.versionDetail.trainingContainerImage.registryId !=
          this.trainingForm.containerImage.registry.id ||
        this.versionDetail.trainingContainerImage.image !=
          this.trainingForm.containerImage.image ||
        this.versionDetail.trainingContainerImage.tag !=
          this.trainingForm.containerImage.tag ||
        this.versionDetail.trainingContainerImage.token !=
          this.trainingForm.containerImage.token ||
        this.versionDetail.trainingGitModel.gitId !=
          this.trainingForm.gitModel.git.id ||
        this.versionDetail.trainingGitModel.repository !=
          this.trainingForm.gitModel.repository.name ||
        this.versionDetail.trainingGitModel.branch !=
          this.trainingForm.gitModel.branch.branchName ||
        this.versionDetail.trainingGitModel.commitId !=
          this.trainingForm.gitModel.commit.commitId ||
        this.versionDetail.trainingGitModel.token !=
          this.trainingForm.gitModel.token ||
        this.versionDetail.trainingEntryPoint != this.trainingForm.entryPoint ||
        this.versionDetail.trainingCpu != this.trainingForm.resource.cpu ||
        this.versionDetail.trainingMemory !=
          this.trainingForm.resource.memory ||
        this.versionDetail.trainingGpu != this.trainingForm.resource.gpu
      ) {
        update = true
      }

      /*
      if (this.versionDetail.evaluationContainerImage == null) {
        if (
          this.evaluationForm.containerImage != null &&
          this.evaluationForm.containerImage.registryId != null
        ) {
          update = true
        }
      } else {
        if (
          this.evaluationForm.containerImage == null ||
          this.versionDetail.evaluationContainerImage.registryId !=
            this.evaluationForm.containerImage.registry.id ||
          this.versionDetail.evaluationContainerImage.image !=
            this.evaluationForm.containerImage.image ||
          this.versionDetail.evaluationContainerImage.tag !=
            this.evaluationForm.containerImage.tag ||
          this.versionDetail.evaluationContainerImage.token !=
            this.evaluationForm.containerImage.token
        ) {
          update = true
        }
      }
      if (this.versionDetail.evaluationGitModel == null) {
        if (
          this.evaluationForm.gitModel != null &&
          this.evaluationForm.gitModel.git != null
        ) {
          update = true
        }
      } else {
        if (
          this.evaluationForm.gitModel == null ||
          this.versionDetail.evaluationGitModel.gitId !=
            this.evaluationForm.gitModel.git.id ||
          this.versionDetail.evaluationGitModel.repository !=
            this.evaluationForm.gitModel.repository.name ||
          this.versionDetail.evaluationGitModel.branch !=
            this.evaluationForm.gitModel.branch.branchName ||
          this.versionDetail.evaluationGitModel.commitId !=
            this.evaluationForm.gitModel.commit.commitId ||
          this.versionDetail.evaluationGitModel.token !=
            this.evaluationForm.gitModel.token
        ) {
          update = true
        }
      }
      if (
        this.versionDetail.evaluationEntryPoint !=
          this.evaluationForm.entryPoint ||
        this.versionDetail.evaluationCpu != this.evaluationForm.resource.cpu ||
        this.versionDetail.evaluationMemory !=
          this.evaluationForm.resource.memory ||
        this.versionDetail.evaluationGpu != this.evaluationForm.resource.gpu
      ) {
        update = true
      }
      */

      if (update == false) {
        if (updatedBase) {
          await this.$notify.success({
            type: 'Success',
            message: `更新しました`,
          })
        }
        return
      }

      const params = {
        preprocessEntryPoint: this.preprocForm.entryPoint,

        preprocessCpu: this.preprocForm.resource.cpu,
        preprocessMemory: this.preprocForm.resource.memory,
        preprocessGpu: this.preprocForm.resource.gpu,

        trainingEntryPoint: this.trainingForm.entryPoint,

        trainingCpu: this.trainingForm.resource.cpu,
        trainingMemory: this.trainingForm.resource.memory,
        trainingGpu: this.trainingForm.resource.gpu,

        //evaluationEntryPoint: this.evaluationForm.entryPoint,

        //evaluationCpu: this.evaluationForm.resource.cpu,
        //evaluationMemory: this.evaluationForm.resource.memory,
        //evaluationGpu: this.evaluationForm.resource.gpu,
        evaluationEntryPoint: null,

        evaluationCpu: 1,
        evaluationMemory: 1,
        evaluationGpu: 0,
      }
      if (
        this.preprocForm.containerImage.image != null &&
        this.preprocForm.containerImage.tag != null
      ) {
        params['preprocessContainerImage'] = {
          ...this.preprocForm.containerImage,
        }
      } else {
        params['preprocessContainerImage'] = null
      }
      if (
        this.trainingForm.containerImage.image != null &&
        this.trainingForm.containerImage.tag != null
      ) {
        params['trainingContainerImage'] = {
          ...this.trainingForm.containerImage,
        }
      } else {
        params['trainingContainerImage'] = null
      }
      /*
      if (
        this.evaluationForm.containerImage.image != null &&
        this.evaluationForm.containerImage.tag != null
      ) {
        params['evaluationContainerImage'] = {
          ...this.evaluationForm.containerImage,
        }
      } else {
        params['evaluationContainerImage'] = null
      }
      */
      params['evaluationContainerImage'] = null

      if (this.preprocForm.gitModel.repository != null) {
        params['preprocessGitModel'] = {
          gitId: this.preprocForm.gitModel.git.id,
          repository: this.preprocForm.gitModel.repository.name,
          owner: this.preprocForm.gitModel.repository.owner,
          branch: this.preprocForm.gitModel.branch.branchName,
          commitId: this.preprocForm.gitModel.commit.commitId,
          token: this.preprocForm.gitModel.token,
        }
      } else {
        params['preprocessGitModel'] = null
      }

      if (this.trainingForm.gitModel.repository != null) {
        params['trainingGitModel'] = {
          gitId: this.trainingForm.gitModel.git.id,
          repository: this.trainingForm.gitModel.repository.name,
          owner: this.trainingForm.gitModel.repository.owner,
          branch: this.trainingForm.gitModel.branch.branchName,
          commitId: this.trainingForm.gitModel.commit.commitId,
          token: this.trainingForm.gitModel.token,
        }
      } else {
        params['trainingGitModel'] = null
      }

      /*
      if (this.evaluationForm.gitModel.git != null) {
        params['evaluationGitModel'] = {
          gitId: this.evaluationForm.gitModel.git.id,
          repository: this.evaluationForm.gitModel.repository.name,
          owner: this.evaluationForm.gitModel.repository.owner,
          branch: this.evaluationForm.gitModel.branch,
          commitId: this.evaluationForm.gitModel.commit.commitId,
          token: this.evaluationForm.gitModel.token,
        }
      } else {
        params['evaluationGitModel'] = null
      }
      */
      params['evaluationGitModel'] = null

      //新規バージョン作成
      await this['postByIdVersions']({ id: this.id, model: params })

      await this.$notify.success({
        type: 'Success',
        message: `更新しました`,
      })

      this.changeFlg = false
      this.versionValue = null
      this.preprocForm = null
      this.trainingForm = null
      this.evaluationForm = null
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
