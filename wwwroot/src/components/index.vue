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
                 :key="s.ProjectID[index]"
                 :value="s.ProjectID"
                 
                 >
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
              <el-switch v-model="tallys.DeleteFlag"></el-switch>
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
          style="width: 35%;"
          placeholder="搜尋名稱"
          v-model="filterText">
        </el-input>
        <div class="el_content">
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
          <div class="del_DataShow" v-if="tallys.DeleteFlag">
            <div v-for='(D,index) in DelFlag'> 
                  <el-tag type="danger">{{D.TallyID}}</el-tag>
                  <!-- <span>{{D.TallyName}}</span> -->
                  <el-input
                    v-model="D.TallyName"
                    :disabled="true"
                    size="small"
                    >
                  </el-input>
            </div>
          </div>
        </div>
      
  </el-container>
</template>
<script>
  const defaultTally = {
    TallyName:'',inputeBodr: false,
    TallyLevel:1,TallyID:1,
    trotally:[],id:1,
    TallyParent:0,SortCode:1,
    DeleteFlag: "N"
  };
  const parentId = '';
  const parentIndex = '';
  const parent = '';
   module.exports = {
    props: [
      "sys_path"
    ],
    data() {
      return {
        select:'',
        value: false,
        filterText: '',
        filterDel:'',
        defaultProps: {children:'trotally',label: 'TallyID',key:'id'},
        Props: {children:'trotally',label: 'TallyID',key:'id'},
        tallys:[defaultTally],
        layer:[],
        tallyTag:0,
        selectTally:[],
        TallyLevelTest:[],
        DelFlag:[],
      }
      
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val);
      },
      DelFlag(val){
        //console.log('val =>',typeof val);
        
      },
      select(val){
        console.log('val select',val);
        this.DelFlag =[]
        console.log('DelFlag',this.DelFlag)
      }
    },
    created() {
      axios.post('https://localhost:5001/API/ReadTally',{})
        .then((res) => {
          this.selectTally = res.data.NewDataSet.TallyProject
          })
        .catch((err) => {console.log('失敗',err)})
    },
    methods: {
      contextTally(p){
        let DeleteFlag = [];
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
                  if(tally.DeleteFlag =='Y'){
                    this.DelFlag.push(tally)
                    // this.DelFlag.find((val, i, arr) => {
                    //   if(arr[i].ProjectID != tally.ProjectID)this.DelFlag=[]
                    //   return this.DelFlag
                    // })  
                    // console.log('this.DelFlag',this.DelFlag);
                  }
                return obj
              },{})
              for(let i = Object.keys(tallyObj).length;i>=2;i--){
                console.log(tallyObj);                
                this.tallys = tallyObj[1]
                for(const tally of tallyObj[i]){
                  const parentId = tally.TallyParent
                  const parentIndex = tallyObj[i-1].findIndex(e=>e.TallyID===parentId)
                  const parent = tallyObj[i-1][parentIndex] 
                  parent['trotally'] 
                  ? parent['trotally'].push(tally) //有資料
                  : parent['trotally'] = [tally]   //沒資料Tally為空
                 
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
                  // console.log('children',children.length)
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
        children[index].DeleteFlag ='Y'
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
        const index = children.findIndex((d) => d.TallyID === data.TallyID)
        children[index].inputeBodr = true;
        console.log('edit',children[index]);
      },
      
    }
   }
</script>