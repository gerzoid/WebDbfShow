import { notification } from 'ant-design-vue';

export const showNotification = (type, title, text)=>{
    notification.open({
         type: type,
         message: title,
         duration: 2,
         description: text,
       });
     };
