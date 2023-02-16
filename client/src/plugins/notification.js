import { notification } from 'ant-design-vue';

export const showNotification = (type, title, text, duration=2)=>{
    notification.open({
         type: type,
         message: title,
         duration: duration,
         description: text,
       });
     };
