import axios from 'axios'
import {
  throwErr
} from './utils' // utils 捕捉服务端http状态码的方法
import {
  Message
} from 'element-ui'
// 过滤请求
axios.interceptors.request.use(config => {
  config.timeout = 10 * 1000 // 请求响应时间
  config.headers = {
    'Content-Type': 'application/json; charset=utf-8'
  };
  return config
}, error => {
  return Promise.reject(error)
})
// 添加响应拦截器
axios.interceptors.response.use(
  response => {
    if (response.data.code === 0) { // 服务端定义的响应code码为0时请求成功
      return response.data // 使用Promise.resolve 正常响应
    } else {
      return response.data // Promise.reject()
    }
  },
  error => {
    if (error && error.response) {
      let res = {}
      res.code = error.response.status
      res.msg = throwErr(error.response.status, error.response) // throwErr 捕捉服务端的http状态码 定义在utils工具类的方法      
      Message({
        type: 'error',
        duration: 5 * 1000,
        message: "服务器出现一个Bug,工程师正在抓它呢!"
      })
      return Promise.reject(res)
    }
    return Promise.reject(error)
  }
)
export default function request(method, url, data) { // 暴露 request 给我们好API 管理
  let BaseUrl = API_PATH + url; // 此处地址转换为webpack全局变量地址+请求地址
  method = method.toLocaleLowerCase() // 封装RESTful API的各种请求方式 以 post get delete为例
  if (method === 'post') {
    return axios.post(BaseUrl, data) // axios的post 默认转化为json格式
  } else if (method === 'get') {
    return axios.get(BaseUrl, {
      params: data
    })
  } else if (method === 'delete') {
    return axios.delete(BaseUrl, {
      params: data
    })
  }
}