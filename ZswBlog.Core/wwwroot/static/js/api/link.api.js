import request from './request'

// 获取所有留言
export const getAllFriendLink = (params) => {
  return request('get', `/Link/GetFriendLinks`, params)
}
export const applyFriendLink = (params) => {
  return request('post', `/Link/AddFriendLink`, params)
}
