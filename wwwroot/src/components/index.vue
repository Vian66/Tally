<template>
  <el-container>
      <el-header style="height: 30px;">Tally && TallyParent</el-header>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-fluid">
        <ul class="projectInfo">
          <li>專案:</li>
          <li><el-input
              size="small"
             v-model="input" placeholder="請選擇專案"></el-input>
          </li>
          <li>Tally</li>
          <li><el-switch
                v-model="value"
                active-color="#13ce66"
                inactive-color="#0d6efd">
              </el-switch>
          </li>
          <li>TallyParent</li>
          <div class="right">
            <li>First node:</li>
            <li><el-button size="small" type="primary" @click='addTallyinpute(tallys)'>Add</el-button></li>
           <li>不顯示</li>
            <li>
              <el-switch v-model="value1"></el-switch>
            </li>
            <li>顯示刪除項目</li>
            <li><el-upload
            class="upload-demo"
            action="https://jsonplaceholder.typicode.com/posts/"
            :on-change="handleChange">
            <el-button size="small" type="primary">匯入</el-button>
            <div slot="tip" class="el-upload__tip"></div>
            
          </el-upload></li>
          <li><el-button size="small" type="primary">匯出</el-button></li>
          </div>
          <li><el-button size="small" class="btn" type="primary">Save</el-button></li>
        </ul>
      </div>
    </nav>

    <!-- Tree -->
        <el-input
          placeholder="搜尋名稱"
          v-model="filterText">
        </el-input>
      <div class="elTree">
        <el-tree
          :data="tallys"
          :props="defaultProps"
          node-key="id"
          default-expand-all
          :ref="'tree'"
          draggable
          :expand-on-click-node="false"
          :filter-node-method="filterNode"
          @check-change="handleCheckChange">
              <template #default="{ node, data }">
                <span class="{'inputeBodr':custom-tree-node}">
                          <input type="text" 
                          maxlength="50"
                          size="30"
                          v-model="data.eid" 
                          v-if="data.inputeBodr"
                          @keydown.enter='data.inputeBodr = false'
                          >
                          

                          <el-button size="mini" type="warning" 
                          @click="edit(data)">{{data.eid}}</el-button>
                          <el-input
                          clearable='true'
                          v-model="data.content" 
                          placeholder="輸入内容" 
                          size="small">
                          </el-input>
                            
                          <span>
                            <el-popover
                                ref="popover"
                                placement="right"
                                :width="232"
                                trigger="click"
                              >
                              <el-button 
                              type="primary" 
                              icon="el-icon-plus" circlsize="mini" circle 
                              @click="append(node,data)">
                              </el-button>

                              <el-button 
                              type="info"  
                              icon="el-icon-upload2" circlsize="mini" circle 
                              v-on:click='handleCheckChange(node, data,"up")'>
                              </el-button>

                              <el-button 
                              type="info"
                              icon="el-icon-download" circlsize="mini" circle  
                              v-on:click='handleCheckChange(node, data,"down")'>
                              </el-button>

                              <el-button
                              type="danger" 
                              icon="el-icon-delete-solid" circle 
                              @click="remove(node, data)">
                              </el-button>
                                <template #reference>
                                  <el-button size='mini' icon='el-icon-more'></el-button>
                                </template>
                              </el-popover>
                          </span>
                        </span>
                </template>
                        
                  
        </el-tree>
      </div>
      
  </el-container>
</template>
<script>
  module.exports = {
    props: [
      "sys_path"
    ],
    data() {
      return {
        input: '',
        value: true,
        value1: true,
        filterText: '',
        defaultProps: {children:'trotally',label: 'content',key:'id'},
        tallys:[{content:'1515',inputeBodr: false,level:0,id:1,trotally:[],eid:1}],
        layer:[],
        tallyTag:0
      }
      
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val);
      }
    },
    methods: {
      handleChange(file, fileList) {
        this.fileList = fileList.slice(-3);
      },
      addTallyinpute(t){
          if(this.tallys.length > 0)
            this.tallyTag = this.tallys[this.tallys.length -1].eid;
            let pushTally = {content:'',inputeBodr: false,level:0,id:parseInt(this.tallyTag)+1,trotally:[],eid:parseInt(this.tallyTag)+1};
            this.tallys.push(pushTally);
      },
      handleCheckChange(node,data,direct) {
                  const parent = node.parent;
                  const children = parent.data.trotally || parent.data;
                  const index = children.findIndex((d) =>d.id === data.id);
                  switch(direct){
                    case 'up':
                        if(index >0){
                          let item = children.splice(index, 1)[0]
                              children.splice(index - 1, 0, item)
                              this.tallys = [...this.tallys];
                        }
                    break;  
                    case 'down':
                        if(index <children.length){
                          let item = children.splice(index, 1)[0]
                              children.splice(index + 1, 0, item)
                              this.tallys = [...this.tallys]; 
                        }
                    break; 
                  }
        },
      append(node,data) { 
        let tallyTag = 1;
       if(data.trotally.length >0){
         let aID = data.trotally[data.trotally.length-1].eid.split('-');
          tallyTag += parseInt(aID[aID.length-1]);
       }
        const newChild = {content:'',inputeBodr: false,level:1,id:data.id+'-'+(tallyTag),trotally:[],eid:data.id+'-'+(tallyTag)}
        if (!data.trotally)data.trotally = [];//沒有資料就給空陣列
        data.trotally.push(newChild);//塞資料
        this.tallys = [...this.tallys];
      },

      remove(node, data) {
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex(d => d.id === data.id);
        children.splice(index, 1);
        this.tallys = [...this.tallys];
      },
      filterNode(value, data) {
        if (!value) return true;
        return data.content.indexOf(value) !== -1;//回傳 -1，表示找不到
      },
      edit(d){
        console.log(d)
        d.inputeBodr = true;
      }
    }
  }
</script>