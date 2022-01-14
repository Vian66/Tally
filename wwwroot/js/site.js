﻿var __vm;

window.onload=function(){
    _vue_init();
}

function _vue_init() {
    var Main = {
        data() {
          return {
            sys_path: ''
          };
        },
		components: {
			'test-component': httpVueLoader('../src/components/test.vue')
		},
        methods: {
          handleClick(tab, event) {
            console.log(tab, event);
          }
        }
      };

    var Ctor = Vue.extend(Main)
    __vm = new Ctor().$mount('#_vue_workplace')
}