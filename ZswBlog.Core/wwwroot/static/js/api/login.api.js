import request from './request'

// 获取所有留言
export const getQQUserInfo = (accessToken, params) => {
  return request('get', `/Login/GetQQUserInfo?accessToken=${accessToken}`, params)
}
// 获取所有留言
export const addUserEmail = (params) => {
  return request('post', `/Login/AddEmail`, params)
}