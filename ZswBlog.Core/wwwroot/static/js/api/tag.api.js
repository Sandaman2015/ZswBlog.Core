import request from './request'

// 获取所有标签
export const getAllTags = (params) => {
  return request('get', `/Details/GetAllTags`, params)
}
export const getTagsAndArticles = (params) => {
  return request('get', `/Tag/GetTagList`, params)
}
export const getArticlesByTagId = (id, params) => {
  return request('get', `/Tag/GetArticleListByTagId/${id}`, params)
}

