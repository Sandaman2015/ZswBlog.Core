import request from './request' // 引入axios的封装方法
// 分页获取文章信息
export const getArticlesByPage = (limit, pageIndex, params) => {
  return request('get', `/Whisper/GetArticlesByPage?limit=${limit}&pageIndex=${pageIndex}`, params)
}

// export const register = (params) => {
//   return request('post', ''/Article/GetArticles', params) //添加管理员
// }
// 获取点赞次数前五的文章
export const getArticleOnLike = (params) => {
  return request('get', `/Whisper/GetArticlesOnTopLike`, params)
}
// 获取浏览次数前五的文章
export const getArticleOnVisit = (params) => {
  return request('get', `/Whisper/GetArticlesOnTopVisit`, params)
}
// 获取文章类别
export const getArticleOnClassType = (params) => {
  return request('get', `/Whisper/GetArticlesOnClassType`, params)
}
// 根据类型获取分页文章
export const getArticlesPageByClass = (limit, pageIndex, classType, params) => {
  return request('get', `/Whisper/GetArticlesByClassPage?limit=${limit}&pageIndex=${pageIndex}&classType=${classType}`, params)
}
// 模糊查询文章
export const getArticleByDimTitle = (title, params) => {
  return request('get', `/Whisper/GetArticlesOnDimTitle/${title}`, params)
}
// 获取单个文章
export const getArticleById = (id, params) => {
  return request('get', `/Details/GetArticle/${id}`, params)
}
// 添加文章的点赞数
export const addArticleLike = (id, params) => {
  return request('get', `/Details/AddArticleLike/${id}`, params)
}
