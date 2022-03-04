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
          <li>顯示刪除項目</li>
           <li>不顯示</li>
            <li>
              <el-switch v-model="tallys.DeleteFlag"></el-switch>
            </li>
            <li>顯示</li>
            <!-- <li>Lang:</li>
            <li>Zh</li>
            <li>
              <el-switch v-model="LocaleID"></el-switch>
            </li>
            <li>En</li> -->
            <li>First node:</li>
            <li><el-button size="small" type="primary" @click='addTallyinpute(tallys)'>Add</el-button></li>
            <li><el-upload
            class="upload-demo"
            action="https://jsonplaceholder.typicode.com/posts/"
            :on-change="handleChange">
            <el-button size="small" type="primary">匯入</el-button>
            <div slot="tip" class="el-upload__tip"></div>
            
          </el-upload></li>
          <li><el-button size="small" type="primary">匯出</el-button></li>
          </div>
          <li><el-button size="small" class="btn" type="primary"
                @click="SaveTally()"
              >Save</el-button></li>
        </ul>
      </div>
    </nav>
     <!-- {{tallys}} -->
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
                              @focus="eTallyName(node,data)"
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
    DeleteFlag:'N',ProjectID:'',
    LocaleID:'',SortCode:1,
    NodeType:'DOCUMENT',FullName:'',
    UserID:'tpp06651',Type:'insert'
  };
  const Param = {
               "ProjectID":"180"
              ,"LocaleID":"zh-tw"
              ,"TallyID" :"400"
              ,"TallyName" :"api success888"
              ,"FullName" : "api success888"
              ,"TallyLevel" : "1"
              ,"TallyParent" : "0"
              ,"SortCode" : "4"
              ,"DeleteFlag" :"N"
              ,"NodeType" : "DOCUMENT"
              ,"UserID" : "tpp06651"
              ,"Type" :"insert"
            };
  function Temptable(){
      return new Promise((resolve,reject)=>{
        axios.post('https://localhost:5001/API/SaveTally',Param)
        .then((res) => {
          console.log('成功Temp',res)
        })
        .catch((err) => {reject(err) })   
       
      })
  }
  function Tallytable(){
      return new Promise((resolve,reject)=>{
        axios.post('https://localhost:5001/API/SaveTallyComplete')
        .then((res) => {
          console.log('成功Tally',res)
        })
        .catch((err) => {reject(err) })   
       
      })
  }
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
        LocaleID:'en-us'
      }
      
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val);
      },
      select(val){
        console.log('val select',val);
        this.DelFlag =[];
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
                LocaleID:'zh-tw'
                //LocaleID:this.LocaleID

        })
        .then((res) => {
          //console.log('成功select',res)
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
                if(tally.DeleteFlag =='N'){
                  obj[tally.TallyLevel]
                    ? obj[tally.TallyLevel].push(tally) 
                    : obj[tally.TallyLevel] = [tally]
                }else{this.DelFlag.push(tally)}
               
                return obj
              },{})
              for(let i = Object.keys(tallyObj).length;i>=1;i--){
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
     async SaveTally(){  
        const Temptable =  Temptable()
          //console.log('Temp',Temp);
        const Tallytable = await Tallytable()
          //console.log('Tally',Tally);
      },
      handleChange(file, fileList) {
        this.fileList = fileList.slice(-3);
      },
      addTallyinpute(t){
          if(t.length > 0)
            this.tallyTag = t[t.length -1].TallyID;
            if(!this.select){
              this.$message({message:'請選擇欲加入的專案',duration:1500})
            }else{
              let pushTally = JSON.parse(JSON.stringify(defaultTally));
              pushTally.TallyID = parseInt(this.tallyTag+1) ;
              pushTally.id = parseInt(this.tallyTag+1);
              pushTally.ProjectID = this.select;
              t.push(pushTally);
            }   
      },
      handleCheckChange(node,data,direct) {
                  const parent = node.parent;
                  const children = parent.data.trotally || parent.data;
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
        if (!data.trotally)data.trotally = [];//沒有資料就給空陣列
        let tallyTag = 1;
         if(data.trotally.length >0){
          let aID = data.trotally[data.trotally.length-1].TallyID;
          tallyTag += parseInt(aID[aID.length-1]);
         }
        let pushChild = JSON.parse(JSON.stringify(defaultTally));
        pushChild.TallyID = data.TallyID+'0'+(tallyTag);
        pushChild.id = data.TallyID+'0'+(tallyTag);
        data.trotally.push(pushChild);//塞資料
        this.tallys = [...this.tallys];
        this.test()
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
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex((d) => d.TallyID === data.TallyID)
        children[index].inputeBodr = true;
        console.log('edit',children[index]);
      },
      eTallyName(node,data){
        console.log('eTallyName');
        if(data.TallyName !='' && data.TallyID){
          const parent = node.parent;
          const children = parent.data.trotally || parent.data;
          const index = children.findIndex((d) => d.TallyID === data.TallyID)
          children[index].Type ='Update'
        }else if(!data.TallyID)children[index].Type ='Insert'
      }
    }
   }
</script>