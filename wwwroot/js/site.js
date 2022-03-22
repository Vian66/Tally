window.onload=function(){
    _vue_init();
}
var Ctor;
let __vm ;
function _vue_init() {
   
    var Main = {
        data() {
          return {
            sys_path: 'sys_path'
          };
        }, 
        components: {
          'index-component': httpVueLoader('../src/components/index.vue')

        },
        methods: {
          handleClick(tab, event) {
            console.log(tab, event);
          }
        }
      };   
       Ctor = Vue.extend(Main)
       __vm = new Ctor().$mount('#_vue_workplace')
}
    


