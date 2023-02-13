import { defineStore } from 'pinia'

export const useDbfShow = defineStore('dbfshow', {
    state: () => ({
      fileName: '',
      pageSize: 10,
      page:1,
      isLoading: false,
      nextId: 0,
    }),
    getters: {
      getFilename(state) {
        return state.fileName;
      },
    },
    actions: {
      // any amount of arguments, return a promise or not
      saveFileName(text) {
        // you can directly mutate the state
        this.fileName= text;
        console.log(text);
      },
    },
  })