import request from './request'
//获取所有评论
export const getAllSiteTag = (params) => {
  return request('get', `/About/GetAllSiteTag`, params)
}
export const getAllTimeLine = (params) => {
  return request('get', `/About/GetAllTimeLine`, params)
}
export const addSiteTag = (params) => {
  return request('post', `/About/AddSiteTag`, params)
}
