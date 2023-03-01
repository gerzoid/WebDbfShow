import {axio} from './axios';
import { getCookie, setCookie } from "./cookies";
import { useFileStore } from "../stores/filestore";

export default class Api {

    //Проверитьь список загруженных файлов по юзеру
    static CheckUploadedFiles() {
        const data = new FormData();
        data.append("userId", getCookie("dbfshowuser"));
        return axio
          .post("/api/users/check", data, { proxy: false })
    }

    //Открыть файл
    static OpenFile(id){
        var formData = new FormData();
        formData.append("fileId", id);
        return axio.post("/api/Files/open", formData);
    }

    //Статистика
    static GetStatistics(){
        const fileStore = useFileStore();
        const data = new FormData();
        data.append("fileName", fileStore.fileInfo.name);
        return axio.post("/api/report/statistics", data);
    }

    //Получить данные
    static GetData(){
        const fileStore = useFileStore();
        var data={
            "FileName" :fileStore.fileInfo.name,
            "PageSize" : fileStore.options.pageSize,
            "Page": fileStore.options.page,
            "Options":fileStore.options
        }
        return axio.post("/api/editor/getData", data);
    }

    //Получить данные
    static SetEncoding(codePageId){
        const fileStore = useFileStore();
        const data = new FormData();
        data.append("FileName", fileStore.fileInfo.name);
        data.append("CodePageId", codePageId);
        return axio.post("/api/editor/encoding", data);
    }

    //Изменение данных
    static Change(result){
        return axio.post("/api/editor/modify", result);
    }

    //Загрузка файла
    static UploadFile(file){
        const fileStore = useFileStore();
        var formData = new FormData();
        formData.append("formfile", file);
        formData.append("filename", file.name);
        formData.append("userId", fileStore.userId);              
        return axio
        .post("/api/Files", formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });
    }

}