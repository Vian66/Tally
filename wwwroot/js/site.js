window.onload=function(){
    _vue_init();
}
//import { defineAsyncComponent } from '../src/components/index.vue'
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
    var Ctor = Vue.extend(Main)
    let __vm = new Ctor().$mount('#_vue_workplace')
  
}


