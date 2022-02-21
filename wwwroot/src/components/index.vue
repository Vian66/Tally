<template>
  <el-container>
      <el-header style="height: 30px;">Tally && TallyParent</el-header>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-fluid">
        <ul class="projectInfo">
          <li>專案:</li>
          <li>
            <el-select v-model="select" placeholder="請選擇專案"
                filterable
                @change="contextTally"
            >
                 <el-option
                 v-for="(s,index) in selectTally"
                 :label="s.ProjectName"
                 :key="s.ProjectID + index"
                 :value="s.ProjectID">
                </el-option>
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
          :render-after-expand="true"
          :expand-on-click-node="false"
          :filter-node-method="filterNode"
          @check-change="handleCheckChange">
              <template #default="{ node, data }">
                <span class="{'inputeBodr':custom-tree-node}">

                          <input type="text" 
                          maxlength="50"
                          v-model="data.TallyID" 
                          v-show="data.inputeBodr"
                          @keydown.enter='data.inputeBodr = false'
                          
                          >
                          <!-- @keydown.enter='data.inputeBodr = false' -->
                          <!--  v-if="data.inputeBodr" -->
                          <el-button size="mini" type="warning"
                          @click="edit(node,data)"
                          >{{data.TallyID}}</el-button>
                          <el-input
                          :clearable='true'
                          v-model="data.TallyName" 
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
  const defaultTally = {
    TallyName:'',inputeBodr: false,
    TallyLevel:1,TallyID:1,
    trotally:[],id:1,
    TallyParent:0,SortCode:1,
  }
   module.exports = {
    props: [
      "sys_path"
      //'async-webpack-example'
    ],
    data() {
      return {
        select: '',
        value: true,
        value1: true,
        filterText: '',
        defaultProps: {children:'trotally',label: 'TallyID',key:'id'},
        tallys:[defaultTally],
        layer:[],
        tallyTag:0,
        selectTally:[],
        TallyLevelTest:[]
      }
      
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val);
      },
    },
    created() {
      axios.post('https://localhost:5001/API/ReadTally',{})
        .then((res) => {this.selectTally = res.data.NewDataSet.TallyProject})
        .catch((err) => {console.log('失敗',err)})
    },
    methods: {
      contextTally(p){
        axios.post('https://localhost:5001/API/SelectTally',{
          ProjectID:p,
                LocaleID:'zh-TW'
        })
        .then((res) => {
          console.log('成功select',res)
            console.log('this.tallys',this.tallys)
             if(!res.data.NewDataSet?.QueryTally){
               //console.log('this.tallys',this.tallys)
                let alert = this.$notify({
                  title: '訊息',
                  message: `此專案ProjectID:${p}暫無資料`,
                  type: 'warning'
                });
                this.tallys = [defaultTally];
                return alert 
              }

              const tallyObj =  res.data.NewDataSet.QueryTally.reduce((obj,tally)=>{
                tally['inputeBodr'] = false;
                obj[tally.TallyLevel]
                  ? obj[tally.TallyLevel].push(tally) 
                  : obj[tally.TallyLevel] = [tally]
                return obj
              },{})

              for(let i = Object.keys(tallyObj).length;i>=2;i--){
                //if(i===1)break  
                //console.log(tallyObj);                
                this.tallys = tallyObj[1]
                for(const tally of tallyObj[i]){
                  const parentId = tally.TallyParent
                  const parentIndex = tallyObj[i-1].findIndex(e=>e.TallyID===parentId)
                  const parent = tallyObj[i-1][parentIndex] 
                  parent['trotally'] 
                  ? parent['trotally'].push(tally) 
                  : parent['trotally'] = [tally]
                }
              }
        })
        .catch((err) => {
          console.log('失敗',err)  
        })   
      },
      handleChange(file, fileList) {
        this.fileList = fileList.slice(-3);
      },
      addTallyinpute(t){
          if(this.tallys.length > 0)
            this.tallyTag = this.tallys[this.tallys.length -1].TallyID;
            let pushTally = {
              TallyName:'',inputeBodr: false,
              TallyLevel:1,TallyID:parseInt(this.tallyTag)+1,
              trotally:[],id:parseInt(this.tallyTag)+1,
              TallyParent:0,SortCode:1};
            this.tallys.push(pushTally);
      },
      handleCheckChange(node,data,direct) {
                  console.log('node',node)
                  console.log('data',data)
                  const parent = node.parent;
                  const children = parent.data.trotally || parent.data;
                  console.log('children',children.length)
                  const index = children.findIndex((d) => d.TallyID === data.TallyID);
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
         let aID = data.trotally[data.trotally.length-1].TallyID;
          tallyTag += parseInt(aID[aID.length-1]);
       }
        const newChild = {TallyName:'',inputeBodr: false,
        TallyLevel:2,TallyID:data.TallyID+'0'+(tallyTag),
        trotally:[],id:data.TallyID+'0'+(tallyTag),SortCode:1}
        if (!data.trotally)data.trotally = [];//沒有資料就給空陣列
        data.trotally.push(newChild);//塞資料
        this.tallys = [...this.tallys];
      },

      remove(node, data) {
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex(d => d.TallyID === data.TallyID);
        children.splice(index, 1);
        this.tallys = [...this.tallys];
      },
      filterNode(value, data) {
        if (!value) return true;
        return data.TallyName.indexOf(value) !== -1;//回傳 -1，表示找不到
      },
      edit(node,data){
        //console.log(data)
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        //console.log('children',children);
        const index = children.findIndex((d) => d.TallyID === data.TallyID)
        children[index].inputeBodr = true;
        console.log('edit',children[index]);
      },

    }
   }
</script>