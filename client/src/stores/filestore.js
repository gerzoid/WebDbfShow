import { defineStore } from 'pinia'
import { codepages } from "../plugins/codepages";

export const useFileStore = defineStore('fileStore', {
    state: () => ({
      fileInfo: [],
      options: {
        page:1,
        pageSize:50,
        hiddenDeletedRecords:false,
      },
      fileName: 'test.dbf',
      userId:'',
      // pageSize: 50,
      // page:1,
      originalFileName:'',
      isLoading: false,
      nextId: 0,
      listUploadedFiles: null,
      needReload: false,
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
      },
      getListUploadedFiles(state){
        return state.listUploadedFiles;
      },
      getCodePage(state){
        return  codepages[state.fileInfo.codePageId];
      },
      getNeedReliad(state){
        return state.needReload;
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