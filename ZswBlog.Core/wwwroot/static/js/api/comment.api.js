import request from './request'
//获取所有评论
export const getAllComment = (articleId, limit, pageIndex, params) => {
  return request('get', `/Details/GetCommentsByPage?articleId=${articleId}&limit=${limit}&pageIndex=${pageIndex}`, params)
}
// 添加留言
export const addComment = (params) => {
  return request('post', `/Details/AddComment`, params)
}