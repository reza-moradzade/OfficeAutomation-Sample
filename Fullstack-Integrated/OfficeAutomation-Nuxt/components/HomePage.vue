<template>
  <div class="home-page">
    <div class="page-header">
      <h1>Dashboard 🏠</h1>
      <p>Welcome to Office Automation System</p>
    </div>

    <div class="stats-cards">
      <div class="stat-card">
        <div class="stat-icon">📊</div>
        <div class="stat-info">
          <h3>125</h3>
          <p>Total Documents</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">👥</div>
        <div class="stat-info">
          <h3>42</h3>
          <p>Current Inbox</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">💰</div>
        <div class="stat-info">
          <h3>8</h3>
          <p>Unread</p>
        </div>
      </div>
      
      <div class="stat-card">
        <div class="stat-icon">✅</div>
        <div class="stat-info">
          <h3>15</h3>
          <p>Urgent</p>
        </div>
      </div>
    </div>

    <div class="recent-activities">
      <div class="section-card">
        <h2>Recent Activities</h2>
        <div class="activities-list">
          <div v-for="activity in activities" :key="activity.id" class="activity-item">
            <div class="activity-icon">{{ activity.icon }}</div>
            <div class="activity-content">
              <p class="activity-text">{{ activity.text }}</p>
              <span class="activity-time">{{ activity.time }}</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
// Sample data for recent activities
const activities = ref([
  {
    id: 1,
    icon: '👤',
    text: 'Password changed successfully',
    time: '5 minutes ago'
  },
  {
    id: 2,
    icon: '📝',
    text: 'Document response submitted',
    time: '1 hour ago'
  },
  {
    id: 3,
    icon: '✅',
    text: 'New document registered',
    time: '2 hours ago'
  },
  {
    id: 4,
    icon: '🔔',
    text: 'New message received',
    time: '5 hours ago'
  }
])
</script>

<style scoped>
.home-page {
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

.stats-cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
  display: flex;
  align-items: center;
  gap: 1rem;
  transition: transform 0.3s ease;
}

.stat-card:hover {
  transform: translateY(-5px);
}

.stat-icon {
  font-size: 2rem;
}

.stat-info h3 {
  font-size: 1.8rem;
  margin: 0;
  color: #333;
}

.stat-info p {
  margin: 0.25rem 0 0 0;
  color: #666;
  font-size: 0.9rem;
}

.recent-activities {
  margin-bottom: 2rem;
}

.section-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
}

.section-card h2 {
  margin: 0 0 1rem 0;
  color: #333;
  font-size: 1.3rem;
}

.activities-list {
  /* Removed space-y error */
}

.activity-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 1rem 0;
  border-bottom: 1px solid #f0f0f0;
}

.activity-item:last-child {
  border-bottom: none;
}

.activity-icon {
  font-size: 1.2rem;
  background: #f8f9fa;
  padding: 0.5rem;
  border-radius: 8px;
  width: 40px;
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
}

.activity-content {
  flex: 1;
}

.activity-text {
  margin: 0;
  color: #333;
  font-weight: 500;
}

.activity-time {
  font-size: 0.8rem;
  color: #888;
  margin-top: 0.25rem;
  display: block;
}
</style>