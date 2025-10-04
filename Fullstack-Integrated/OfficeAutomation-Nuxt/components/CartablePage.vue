<template>
  <div class="cartable-page">
    <div class="page-header">
      <h1>Inbox 📋</h1>
      <p>Document Registration and Response</p>
    </div>

    <div class="tasks-section">
      <div class="tasks-header">
        <h2>My Documents</h2>
        <button class="add-task-btn">
          + Add New Document
        </button>
      </div>

      <div class="tasks-grid">
        <div v-for="task in tasks" :key="task.id" class="task-card" :class="task.status">
          <div class="task-header">
            <h3 class="task-title">{{ task.title }}</h3>
            <span class="task-priority" :class="task.priority">
              {{ priorityLabels[task.priority] }}
            </span>
          </div>
          
          <p class="task-description">{{ task.description }}</p>
          
          <div class="task-footer">
            <div class="task-meta">
              <span class="task-date">📅 {{ task.dueDate }}</span>
              <span class="task-assignee">👤 {{ task.assignee }}</span>
            </div>
            <div class="task-actions">
              <button class="action-btn" @click="editTask(task.id)">✏️</button>
              <button class="action-btn" @click="deleteTask(task.id)">🗑️</button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="quick-actions">
      <h2>Quick Actions</h2>
      <div class="actions-grid">
        <button v-for="action in quickActions" :key="action.id" class="action-card">
          <span class="action-icon">{{ action.icon }}</span>
          <span class="action-text">{{ action.text }}</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
// Sample data for documents
const tasks = ref([
  {
    id: 1,
    title: 'Financial Report Review',
    description: 'Review and approve Q1 financial report',
    status: 'pending',
    priority: 'high',
    dueDate: '2024/03/05',
    assignee: 'John Smith'
  },
  {
    id: 2,
    title: 'System Update',
    description: 'Update system to latest version',
    status: 'in-progress',
    priority: 'medium',
    dueDate: '2024/02/28',
    assignee: 'Mike Johnson'
  },
  {
    id: 3,
    title: 'Sales Team Meeting',
    description: 'Review sales team performance for last month',
    status: 'completed',
    priority: 'low',
    dueDate: '2024/02/25',
    assignee: 'Sarah Wilson'
  },
  {
    id: 4,
    title: 'New UI Design',
    description: 'Design user interface for management section',
    status: 'pending',
    priority: 'high',
    dueDate: '2024/03/10',
    assignee: 'David Brown'
  }
])

// Quick actions data
const quickActions = ref([
  { id: 1, icon: '📤', text: 'Create Document' },
  { id: 2, icon: '📊', text: 'Reply to Document' },
  { id: 3, icon: '👥', text: 'Online Message' },
  { id: 4, icon: '⚙️', text: 'Settings' }
])

// Priority labels mapping
const priorityLabels = {
  high: 'Urgent',
  medium: 'Follow-up',
  low: 'Normal'
}

// Document management functions
const editTask = (taskId) => {
  alert(`Edit document ${taskId}`)
}

const deleteTask = (taskId) => {
  if (confirm('Are you sure you want to delete this document?')) {
    tasks.value = tasks.value.filter(task => task.id !== taskId)
  }
}
</script>

<style scoped>
.cartable-page {
  direction: ltr; /* Changed to LTR for English */
}

.page-header {
  margin-bottom: 2rem;
}

.page-header h1 {
  color: #333;
  margin-bottom: 0.5rem;
  font-size: 2rem;
}

.page-header p {
  color: #666;
  font-size: 1.1rem;
}

.tasks-section {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  margin-bottom: 2rem;
}

.tasks-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.tasks-header h2 {
  margin: 0;
  color: #333;
}

.add-task-btn {
  background: #667eea;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  font-weight: 500;
  transition: background 0.3s ease;
}

.add-task-btn:hover {
  background: #5a6fd8;
}

.tasks-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
}

.task-card {
  background: #f8f9fa;
  padding: 1.5rem;
  border-radius: 8px;
  border-left: 4px solid #ddd; /* Changed to left border for LTR */
  transition: all 0.3s ease;
}

.task-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 15px rgba(0, 0, 0, 0.1);
}

.task-card.pending {
  border-left-color: #ffc107; /* Changed to left border */
}

.task-card.in-progress {
  border-left-color: #17a2b8; /* Changed to left border */
}

.task-card.completed {
  border-left-color: #28a745; /* Changed to left border */
}

.task-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 1rem;
}

.task-title {
  margin: 0;
  color: #333;
  font-size: 1.1rem;
  flex: 1;
}

.task-priority {
  font-size: 0.8rem;
  padding: 0.25rem 0.5rem;
  border-radius: 4px;
  font-weight: 500;
}

.task-priority.high {
  background: #f8d7da;
  color: #721c24;
}

.task-priority.medium {
  background: #fff3cd;
  color: #856404;
}

.task-priority.low {
  background: #d1ecf1;
  color: #0c5460;
}

.task-description {
  color: #666;
  margin: 0 0 1rem 0;
  line-height: 1.5;
}

.task-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.task-meta {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.task-date, .task-assignee {
  font-size: 0.8rem;
  color: #888;
}

.task-actions {
  display: flex;
  gap: 0.5rem;
}

.action-btn {
  background: none;
  border: none;
  cursor: pointer;
  padding: 0.25rem;
  border-radius: 4px;
  transition: background 0.3s ease;
}

.action-btn:hover {
  background: rgba(0, 0, 0, 0.1);
}

.quick-actions {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.quick-actions h2 {
  margin: 0 0 1.5rem 0;
  color: #333;
}

.actions-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
  gap: 1rem;
}

.action-card {
  background: #f8f9fa;
  border: 2px dashed #ddd;
  padding: 1.5rem;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.3s ease;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 0.75rem;
}

.action-card:hover {
  border-color: #667eea;
  background: #f0f2ff;
}

.action-icon {
  font-size: 1.5rem;
}

.action-text {
  font-weight: 500;
  color: #333;
}
</style>