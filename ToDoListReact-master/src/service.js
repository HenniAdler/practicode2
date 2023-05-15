import axios from 'axios';

axios.defaults.baseURL = "https://localhost:7001/items"

export default {
  getTasks: async () => {
    const result = await axios.get(``)
    return result.data;
  },

  addTask: async (name) => {
    console.log('addTask', name)
    const result = await axios.post(``, { Name: name, Iscomplete: 0 })
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete })
    const result = await axios.put(`/${id}?isComplete=${isComplete}`)
    return result.data;
  },

  deleteTask: async (id) => {
    console.log('deleteTask')
    const result = await axios.delete(`/${id}`)
    return result.data;
  }
};
axios.interceptors.response.use(function (response) {
  return response;
}, function (error) {
  console.error(error)
  return Promise.reject(error);
});
