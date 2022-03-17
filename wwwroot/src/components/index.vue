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
                @click="SaveTally(tallys)"
              >Save</el-button></li>
        </ul>
      </div>
    </nav>
     {{tallys}}
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
                              <input type="num" 
                              v-model="data.TallyID" 
                              v-if="data.inputeBodr"
                              @keydown.enter="edit2(node,data)"
                              >
                            
                              <!-- @keydown.enter='data.inputeBodr = false' -->
                              <el-button size="mini" type="warning"
                              @click="edit(node,data)"
                              >{{data.TallyID}}</el-button>
                              <el-input
                              :clearable='true'
                              v-model="data.TallyName" 
                              placeholder="輸入内容" 
                              size="small"
                              @change="tName(node,data)" 
                              >
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
  function inDir(e){
    for(i=0;i<e.length;i++){e[i].SortCode = i+1}
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
        tallys:[],
        layer:[],
        tallyTag:0,
        selectTally:[],
        tType:'',
        DelFlag:[],
        LocaleID:'en-us',
        defaultTally : {
          TallyName:'',inputeBodr: false,
          TallyLevel:1,TallyID:1,
          trotally:[],id:1,
          TallyParent:0,SortCode:1,
          DeleteFlag:'N',ProjectID:'',
          LocaleID:'zh-tw',SortCode:1,
          NodeType:'DOCUMENT',FullName:'666',
          UserID:'tpp06651',Type:''
        },
        resData:{}
      }
      
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val);
      },
      select(val){
        this.DelFlag =[];
      },
      tallys:{
          handler(newValue,oldValue) {
            newValue.forEach((val, i, arr) => {
                newValue[i].TallyName
                //console.log('watch',newValue[i].TallyName);
            })
              
          },deep: true
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
          this.resData = res.data.NewDataSet.QueryTally
             if(!res.data.NewDataSet?.QueryTally){
               //console.log('this.tallys',this.tallys)
                let alert = this.$notify({
                  title: '訊息',
                  message: `此專案ProjectID:${p}暫無資料`,
                  type: 'warning'
                });
                this.tallys = [];
                return alert 
            }

              const tallyObj =  res.data.NewDataSet.QueryTally.reduce((obj,tally)=>{
                tally['inputeBodr'] = false;
                tally.Type ='Update';
                tally.FullName = '5555'
                tally.UserID = 'tpp06651'
                if(tally.DeleteFlag =='N'){
                  obj[tally.TallyLevel]
                    ? obj[tally.TallyLevel].push(tally) 
                    : obj[tally.TallyLevel] = [tally]
                }else{this.DelFlag.push(tally)}
                return obj
              },{})
              for(let i = Object.keys(tallyObj).length;i>=1;i--){           
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
      Temptable(){
        this.testTall(this.tallys) 
      },
      testTall(e){
         for(i=0; i<e.length; i++){
          console.log('i',this.tallys[i]);
          axios.post('https://localhost:5001/API/SaveTally',e[i])
           .then((res) => {
              console.log('成功Temp',res)
             })
             .catch((err) => {
              console.log('失敗',err)  
            })  
              if(e[i].trotally)
              if(Object.keys(e[i].trotally).length!=0 && e[i].trotally){
                this.testTall(e[i].trotally)
              }
             
          
          
        }    
      },
      Tallytable(){
      return new Promise((resolve,reject)=>{
        axios.post('https://localhost:5001/API/SaveTallyComplete')
        .then((res) => {
          console.log('成功Tally',res)
        })
        .catch((err) => {reject(err)})   
       
        })
      },
     async SaveTally(t){ 
          console.log('n'); 
         const Temptable = this.Temptable()
         const Tallytable = await this.Tallytable()
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
              let pushTally = JSON.parse(JSON.stringify(this.defaultTally));
              pushTally.TallyID = parseInt(this.tallyTag+1) ;
              pushTally.id = parseInt(this.tallyTag+1);
              pushTally.ProjectID = this.select;
              pushTally.Type = 'Insert'
              t.push(pushTally);
            }   
              inDir(t)
      },
      handleCheckChange(node,data,direct){
                  const parent = node.parent;
                  const children = parent.data.trotally || parent.data;
                  const index = children.findIndex((d) => d.TallyID === data.TallyID);
                  switch(direct){
                    case 'up':
                        if(index >0){
                          let item = children.splice(index, 1)[0]
                              children.splice(index - 1, 0, item)
                              this.tallys = [...this.tallys];
                              inDir(children)
                              
                        }
                    break;  
                    case 'down':
                        if(index <children.length){
                          let item = children.splice(index, 1)[0]
                              children.splice(index + 1, 0, item)
                              this.tallys = [...this.tallys];
                              inDir(children) 
                        }
                    break; 
                  }
      },
      append(node,data) { 
        console.log('append');
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex(d => d.TallyID === data.TallyID);
        if (!data.trotally)data.trotally = [];//沒有資料就給空陣列
        let tallyTag = 1;
        let tallyLevel =1; 
         if(data.trotally.length >0){
          let aID = data.trotally[data.trotally.length-1].TallyID;
          let aLevel = data.trotally[data.trotally.length-1].TallyLevel;
          tallyTag += parseInt(aID[aID.length-1]);
          tallyLevel += parseInt(aLevel[aLevel.length-1]);
         }
        let pushChild = JSON.parse(JSON.stringify(this.defaultTally));
        pushChild.TallyID = data.TallyID+'0'+(tallyTag);
        pushChild.id = data.TallyID+'0'+(tallyTag);
        pushChild.TallyLevel = parseInt(data.TallyLevel) +(tallyLevel);
        //pushChild.TallyLevel = parseInt(data.TallyLevel) +(tallyLevel);
        pushChild.TallyParent = children[index].TallyID
        pushChild.ProjectID = this.select;
        pushChild.Type ='Insert';
        data.trotally.push(pushChild);//塞資料
        this.tallys = [...this.tallys];
      },
      remove(node, data) {
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex(d => d.TallyID === data.TallyID);
        children[index].DeleteFlag ='Y'
        if(children[index].DeleteFlag ='Y')this.DelFlag.push(children[index])
        children.splice(index, 1);
        this.tallys = [...this.tallys];
      },
      filterNode(value, data){
        if (!value) return true;
        return data.TallyName.indexOf(value) !== -1;//回傳 -1，表示找不到
      },
      edit(node,data){
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex((d) => d.TallyID === data.TallyID)
        children[index].inputeBodr = true;
      },
      edit2(node,data){
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex((d) => d.TallyID === data.TallyID)
        children[index].inputeBodr = false;
        children[index].TallyID = Number(children[index].TallyID)
      },
      tName(node,data){
          const parent = node.parent;
          const children = parent.data.trotally || parent.data;
          const index = children.findIndex((d) => d.TallyID === data.TallyID)
            console.log('resData',this.resData);
          if(this.resData ==='undefined'){
            let res = this.resData.indexOf(children[index])
           return res ===-1 ? children[index].Type ='Insert' : children[index].Type ='Update'
          }          
      }
    }
   }
</script>