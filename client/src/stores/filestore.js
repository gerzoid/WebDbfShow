import { defineStore } from 'pinia'

export const useFileStore = defineStore('fileStore', {
    state: () => ({
      fileInfo: [],
      fileName: 'test.dbf',
      pageSize: 50,
      page:1,
      originalFileName:'',
      isLoading: false,
      nextId: 0,
    }),
    getters: {
      getFilename(state) {
        return state.fileName;
      },
      getIsLoading(state){
        return state.isLoading;
      },
      getCountColumns(state){
        return state.fileInfo.countColumns -1; //Вычитаем исусственную колонку _IS_DELETED_
      }
    },
    actions: {
      //Закрываем ооткрытый файл
      closeFile(){
        this.isLoading = false;
        this.fileInfo =[];
        this.fileName ='';
        this.originalFileName ='';
      },
      saveFileName(text) {
        // you can directly mutate the state
        this.fileName= text;
        console.log(text);
      },
    },
  })