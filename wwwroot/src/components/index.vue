<template>
  <el-container>
      <el-header style="height: 30px;">Tally && TallyParent</el-header>
    <nav class="navbar navbar-expand-lg navbar-light bg-light">
      <div class="container-fluid">
        <ul class="projectInfo">
          <li>語系:</li>
          <li>
            <el-select v-model="lacaleID" placeholder="選擇語系">
                 <el-option
                  v-for="(item) in options"
                  :key="item"
                  :label="item"
                  :value="item"
                 >
                </el-option>
          </li>
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
            <li>First node:</li>
            <li><el-button size="small" type="primary" @click='addTallyinpute(tallys)'>Add</el-button></li>
            <li>
              <!-- <el-upload
              class="upload-demo"
              action=""
              :on-preview="handlePreview"
              :before-upload="beforeAvatarUpload"
              :on-success="handleAvatarSuccess"
              :on-change="handleChange">
              <el-button size="small" type="primary">匯入</el-button>
              <div slot="tip" class="el-upload__tip"></div>
                <el-button style="margin-left: 10px;" size="small" type="success" @click="submitUpload">上傳</el-button>  
              </el-upload> -->
             <input type="file" @change="importExcel"/>
          </li>
          <li><el-button size="small" type="primary">匯出</el-button></li>
          </div>
          <li><el-button size="small" class="btn" type="primary"
                @click="SaveTally(tallys,DelFlag)"
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
                               
                              <span class="iconTool" style="border: 1px chartreuse ;">
                                <el-popover
                                    ref="popover"
                                    placement="right"
                                    :width="280"
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
                                    <el-button
                                    type="info" 
                                    icon="el-icon-setting" circle 
                                    @click="channel('show',node, data)">
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
            <el-dialog
                title="Channel"
                :visible.sync="dialogVisible"
                width="30%"
                >
                {{checkboxGroup1}}
                <h6>空白:預設全抓</h6>
                <span>1:Phone  2:Chat  3:Email</span>
                <el-checkbox-group v-model="checkboxGroup1" size="small">
                  <el-checkbox label="1" border></el-checkbox>
                  <el-checkbox label="2" border></el-checkbox>
                  <el-checkbox label="3" border></el-checkbox>
                </el-checkbox-group>
                <span slot="footer" class="dialog-footer">
                  <!-- <el-button @click="dialogVisible = false">取 消</el-button> -->
                  <el-button type="primary" @click="this.dialogVisible = false">确 定</el-button>
                </span>
            </el-dialog>
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
        checkboxGroup1: [],
        dialogVisible: false,
        select:'',
        lacaleID:'',
        options: ['en-us','zh-tw'],
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
        defaultTally : {
          TallyName:'',inputeBodr: false,
          TallyLevel:1,TallyID:1,
          trotally:[],id:1,
          TallyParent:0,SortCode:1,
          DeleteFlag:'N',ProjectID:'',
          LocaleID:'',SortCode:1,
          NodeType:'DOCUMENT',FullName:'',
          UserID:'tpp06651',Type:'',
          Channel:[]
        },
        resData:{},
        dialogFormVisible: false,
        name:'',
        arr:[]
      }
      
    },
    watch: {
      filterText(val) {
        this.$refs.tree.filter(val);
      },
      select(val){
        this.DelFlag =[];
        if(this.lacaleID=="")
        this.$message.error('請選擇語系');
      },
      lacaleID(val){
        //console.log('lang',val);
        //this.contextTally()
      },
      resData(val){
        // console.log('delflag val',val);
        // let delID = this.DelFlag.forEach((val, i, arr) => {return arr[i].TallyID})
        //  if(this.resData){
        //    //this.DelFlag =[];
        //    //console.log('delflag val',this.DelFlag);
        //    this.resData.forEach((val, i, arr) => {
        //       if(arr[i].DeleteFlag=='Y')
        //       arr[i].TallyID.indexOf(this.DelFlag)
        //    })
        //  }
        //  console.log('delID',delID);
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
      flatten(e){
        for(let i=0; i<e.length; i++){
          this.arr.push(e[i]);
          if(Object.keys(e[i].trotally).length!=0){
            this.flatten(e[i].trotally)
          }
        }       
      },
      contextTally(p,d){
        return new Promise((res,rej)=>{
            res(axios.post('https://localhost:5001/API/SelectTally',{ProjectID:p,LocaleID:this.lacaleID}))
            rej('context context context')
        })
        .then((res) => {
          console.log('成功select',res)
          this.resData = res.data.NewDataSet.QueryTally
             if(!res.data.NewDataSet?.QueryTally && this.lacaleID !=""){
                let alert = this.$notify({
                  title: '訊息',
                  message: `此專案ProjectID:${p}暫無資料`,
                  type: 'warning'
                });
                this.tallys = [];
                return alert 
            }
              this.DelFlag = [];
              const tallyObj =  res.data.NewDataSet.QueryTally.reduce((obj,tally)=>{
                tally['inputeBodr'] = false;
                tally.Type ='Update';
                tally.UserID = 'tpp06651';
                tally.trotally =[];
                tally.id = tally.TallyID;
                if(tally.DeleteFlag =='N'){
                  obj[tally.TallyLevel]
                    ? obj[tally.TallyLevel].push(tally) 
                    : obj[tally.TallyLevel] = [tally]
                }else{this.DelFlag.push(tally)}
                return obj
              },{})
              for(let i = Object.keys(tallyObj).length;i>=1;i--){           
                this.tallys = tallyObj[1]
                //console.log('tallyObj',tallyObj);
                for(const tally of tallyObj[i]){
                  const parentId = tally.TallyParent
                  const parentIndex = tallyObj[i-1].findIndex(e=>e.TallyID===parentId)
                  const parent = tallyObj[i-1][parentIndex] 
                  parent['trotally'] 
                  ? parent['trotally'].push(tally) //有資料
                  : parent['trotally'] = [tally]   //沒資料Tally為空
                }
              }
        }).catch((err) => {console.log(err);}) 
      },
      Temptable(t,d){
        return new Promise((res,rej)=>{
            res(this.testTall(this.tallys))
            res(this.testTall(this.DelFlag))
            rej('1Temptable--err')
        })
      },
      testTall(e){
        this.arr = [];
        this.flatten(e)
        console.log('e',e);
        let obj={tallys:this.arr};
          axios.post('https://localhost:5001/API/SaveTally',obj)
           .then((res) => {
              console.log('成功Temp',res)
             })
             .catch((err) => {
              console.log(err)  
            })         
           
      },
      Tallytable(){
        return new Promise((res,rej)=>{
          res(axios.post('https://localhost:5001/API/SaveTallyComplete'))
          rej('2Tally--err')
          .then((res) => {
            console.log('成功Tally',res)
          })
          .catch((err) => {console.log(err);})   
        })
      },
      ClearTemp(){   
        return new Promise((res,rej)=>{
          res(axios.post('https://localhost:5001/API/ClearTemp',{
            ProjectID:this.select,
            LocaleID:this.lacaleID
          }))
          rej('3Clear--err')
          .then((res) => {
            console.log('ClearTemp')
          })
          .catch((err) => {console.log(err);})
        })
       },
     async SaveTally(t,d){ 
      try{
         const Temptable = await this.Temptable()
         const Tallytable = await this.Tallytable()
         const ClearTemp = await this.ClearTemp()
         const contextTally = await this.contextTally(s,d)
        }catch(err){console.log('save-err',err)}
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
              pushTally.TallyID = parseInt(this.tallyTag+1);
              pushTally.id = parseInt(this.tallyTag+1);
              pushTally.ProjectID = this.select;
              pushTally.Type = 'Insert';
              pushTally.LocaleID = this.lacaleID
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
        console.log('append',data);
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex(d => d.TallyID === data.TallyID);
        if(!data.trotally)data.trotally = [];//沒有資料就給空陣列
        let tallyTag = 1;
        let aID; 
         if(data.trotally.length >0){
          aID = parseInt(data.trotally[data.trotally.length-1].TallyID);
          tallyTag += parseInt(aID[aID.length-1]);
         }
        let pushChild = JSON.parse(JSON.stringify(this.defaultTally));
        if(children[index].trotally.length==0 && children[index].TallyID){
          console.log('id next');
          pushChild.TallyID = data.TallyID+'0'+(tallyTag);
          pushChild.id = data.TallyID+'0'+(tallyTag);
        }else if(children[index].trotally.length>0 && children[index].TallyID){
          aID++
          pushChild.TallyID = aID;
          pushChild.id = aID;
        }
        console.log('Name',pushChild.TallyName);
        console.log('Name11',data.TallyName);
        pushChild.TallyParent = children[index].TallyID
        pushChild.ProjectID = this.select;
        pushChild.Type ='Insert';
        pushChild.LocaleID = this.lacaleID;
        if(children[index].trotally.length!=0 && children[index].SortCode)
        pushChild.SortCode += parseInt(data.trotally[data.trotally.length-1].SortCode);
        if(children[index].trotally.length==0 && children[index].TallyParent==0 ||children[index].trotally.length>0 && children[index].TallyParent==0){
          pushChild.TallyLevel +=1
        }else if(children[index].trotally.length==0 && children[index].TallyParent!=0 ||children[index].trotally.length>0 && children[index].TallyParent!=0){
          pushChild.TallyLevel = parseInt(children[index].TallyLevel) + 1
        }
        data.trotally.push(pushChild);//塞資料
        //this.$set(data.trotally, data.trotally.length, pushChild);
        const childInd = data.trotally[data.trotally.length-1]
        console.log('childInd',childInd.TallyName);
        this.tallys = [...this.tallys];
        //this.$forceUpdate();
        //this.$set(data.trotally, data.trotally.length, pushChild);
      },
      remove(node, data) {
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex(d => d.TallyID === data.TallyID);
        children[index].DeleteFlag ='Y'   
        if(children[index].TallyName !='')this.DelFlag.push(children[index])
          if(Object.keys(children[index].trotally).length!=0){
            Deltrotally(children[index].trotally)  
          }
        function Deltrotally(e){
          for(let i=0;i<Object.keys(e).length;i++){
            e[i].DeleteFlag ='Y';
               console.log('this.DelFlag',__vm.$refs.indexcomponent.DelFlag);
               __vm.$refs.indexcomponent.DelFlag.push(e[i])
               if(e[i].trotally.length!=0)
               Deltrotally(e[i].trotally)
            }
            }
          //  this.DelFlag.forEach((val, i, arr) => {
          //     console.log();
          //  })
           children.splice(index, 1);
           this.tallys = [...this.tallys];
      },
      filterNode(value, data){
        if (!value) return true;
        return data.TallyName.indexOf(value) !== -1;//回傳 -1，表示找不到
      },
      edit(node,data){
        console.log('ddd');
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
          this.name = children[index].TallyName
          if(children[index].TallyLevel ==1){
            children[index].FullName = children[index].TallyName;
          }
          if(children[index].TallyLevel ==2){
            parInd = parseInt(children[index].TallyParent);
            children[index].FullName = `[${parent.data.FullName}].[${children[index].TallyName}]`
          }
          if(children[index].TallyLevel >2){ 
            children[index].FullName = `${parent.data.FullName}.[${children[index].TallyName}]`
          }
          if(this.resData ==='undefined'){
            let res = this.resData.indexOf(children[index])
           return res ===-1 ? children[index].Type ='Insert' : children[index].Type ='Update'
          }       
      },
      channel(e,node,data){
        console.log(node);
        console.log(data);
        this.dialogVisible = true;
        this.comfirm(node,data)
        // const parent = node.parent;
        // const children = parent.data.trotally || parent.data;
        // const index = children.findIndex((d) => d.TallyID === data.TallyID)
              //children[index].Channel = this.checkboxGroup1
               
              
          // switch(e){
          //     case 'show':
          //       console.log('show');
          //       //this.dialogVisible = true;
          //       this.comfirm(node,data)
          //       break;
          //     case 'close':
          //       //console.log('close');
          //       this.dialogVisible = false;
          //       break;

          // }
      },
      comfirm(node,data){
        const parent = node.parent;
        const children = parent.data.trotally || parent.data;
        const index = children.findIndex((d) => d.TallyID === data.TallyID)
        console.log('chnl',children[index].Channel); 
        console.log('2chnl',children[index]); 
      },
      beforeAvatarUpload(file) {
        let Xls = file.name.split('.');
        if(Xls[1] === 'xls'||Xls[1] === 'xlsx'){
          return file
        }else {
          this.$message.error('上傳檔案只能是 xls/xlsx 格式!')
        return false
        }
      },
      handleAvatarSuccess(res,file) {
        const files = URL.createObjectURL(file.raw);
          console.log('upload res',files);
          
        //this.imageUrl = URL.createObjectURL(file.raw);
      },
      importExcel(obj){
          let wb; //读取完成的数据
          let aa = [];
          let text = [];
          let rABS = false; //是否将文件读取为二进制字符串
          const file = obj.target.files[0];
          console.log('importExcel',file);
          const Xls = file.name.split('.');
          const IMPORTFILE_MAXSIZE = 1 * 2048; //这里可以自定义控制导入文件大小
          if(Xls[1] === 'xls'||Xls[1] === 'xlsx'){
            let reader = new FileReader();
            console.log('reader',reader);
            reader.onload = function (e) {
                let data = e.target.result;
                if (rABS) {
                    wb = XLSX.read(btoa(fixdata(data)), { //手动转化
                        type: 'base64'
                    });
                } else {
                    wb = XLSX.read(data, {
                        type: 'binary'
                    });
                }
                //wb.SheetNames[0]是获取Sheets中第一个Sheet的名字
                //wb.Sheets[Sheet名]获取第一个Sheet的数据
                this.tallys = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb.SheetNames[0]]));
                console.log('this.tallys',this.tallys);
                var u = eval('(' + this.tallys + ')');
                // document.getElementById("demo").innerHTML = JSON.stringify(XLSX.utils.sheet_to_json(wb.Sheets[wb
                //     .SheetNames[0]]));
                console.log(wb.Sheets[wb.SheetNames[0]])
                //获取表格中为address的那列存入text中
                for (var i = 0; i < u.length; i++) {
                    text.push(u[i].address);
                }

            };
            if (rABS) {
                reader.readAsArrayBuffer(file);
            } else {
                reader.readAsBinaryString(file);
            }
            return file
          }else {
            this.$message.error('上傳檔案只能是 xls/xlsx 格式!')
          return false
          }
          if (file.size / 1024 > IMPORTFILE_MAXSIZE) {
            this.$message.error('导入的表格文件不能大于2M')
            return false
          }
          

      }
      
    }
   }
</script>