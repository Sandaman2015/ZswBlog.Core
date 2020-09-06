import request from './request'

// 获取所有留言
export const getAllMessage = (limit, pageIndex, params) => {
  return request('get', `/Leactos/GetMessagesByPage?limit=${limit}&pageIndex=${pageIndex}`, params)
}
// 添加留言
export const addMessage = (params) => {
  return request('post', `/Leactos/AddMessage`, params)
}
export const getLocation = (ip, params) => {
  return request('get', `/Leactos/GetLocationByIp?ip=${ip}`, params)
}